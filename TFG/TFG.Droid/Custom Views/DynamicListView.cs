using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Animation;
using Java.Util;
using System.Collections;
using Java.Lang;
using static Android.Widget.AbsListView;
using TFG.Droid.Adapters;

namespace TFG.Droid.Custom_Views {

    /**
     * The dynamic listview is an extension of listview that supports cell dragging
     * and swapping.
     *
     * This layout is in charge of positioning the hover cell in the correct location
     * on the screen in response to user touch events. It uses the position of the
     * hover cell to determine when two cells should be swapped. If two cells should
     * be swapped, all the corresponding data set and layout changes are handled here.
     *
     * If no cell is selected, all the touch events are passed down to the listview
     * and behave normally. If one of the items in the listview experiences a
     * long press event, the contents of its current visible state are captured as
     * a bitmap and its visibility is set to INVISIBLE. A hover cell is then created and
     * added to this layout as an overlaying BitmapDrawable above the listview. Once the
     * hover cell is translated some distance to signify an item swap, a data set change
     * accompanied by animation takes place. When the user releases the hover cell,
     * it animates into its corresponding position in the listview.
     *
     * When the hover cell is either above or below the bounds of the listview, this
     * listview also scrolls on its own so as to reveal additional content.
     */

    class DynamicListView : ListView, ITypeEvaluator, IOnScrollListener {

        private readonly int SMOOTH_SCROLL_AMOUNT_AT_EDGE = 15;
        private readonly int MOVE_DURATION = 150;
        private readonly int LINE_THICKNESS = 15;

        public List<View> _viewList;

        private int _lastEventY = -1;

        private int _downY = -1;
        private int _downX = -1;

        private int _totalOffset = 0;

        private bool _cellIsMobile = false;
        private bool _isMobileScrolling = false;
        private int _smoothScrollAmountAtEdge = 0;

        private static readonly int INVALID_ID = -1;
        private long _aboveItemId = INVALID_ID;
        private long _mobileItemId = INVALID_ID;
        private long _belowItemId = INVALID_ID;

        private BitmapDrawable _hoverCell;
        private Rect _hoverCellCurrentBounds;
        private Rect _hoverCellOriginalBounds;

        private static readonly int INVALID_POINTER_ID = -1;
        private int _activePointerId = INVALID_POINTER_ID;

        private bool _isWaitingForScrollFinish = false;
        private int _scrollState = (int) ScrollState.Idle;

        private int _previousFirstVisibleItem = -1;
        private int _previousVisibleItemCount = -1;
        private int _currentFirstVisibleItem;
        private int _currentVisibleItemCount;
        private int _currentScrollState;

        public DynamicListView(Context context) : base(context) {
            init(context);
        }

        public DynamicListView(Context context, IAttributeSet attrs) : base(context, attrs) {
            init(context);
        }

        public DynamicListView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) {
            init(context);
        }

        public void init(Context context) {
            this.LongClick += OnItemLongClick; 
            DisplayMetrics metrics = context.Resources.DisplayMetrics;
            _smoothScrollAmountAtEdge = (int) (SMOOTH_SCROLL_AMOUNT_AT_EDGE / metrics.Density);
        }

        /**
         * Listens for long clicks on any items in the listview. When a cell has
         * been selected, the hover cell is created and set up.
         */
        private void OnItemLongClick(object sender, EventArgs e) {
            _totalOffset = 0;

            int position = PointToPosition(_downX, _downY);
            int itemNum = position - FirstVisiblePosition;

            View selectedView = GetChildAt(itemNum);
            _mobileItemId = Adapter.GetItemId(position);
            _hoverCell = GetAndAddHoverView(selectedView);
            selectedView.Visibility = ViewStates.Invisible;

            _cellIsMobile = true;

            UpdateNeighborViewsForID(_mobileItemId);
        }


        /**
        * Creates the hover cell with the appropriate bitmap and of appropriate
        * size. The hover cell's BitmapDrawable is drawn on top of the bitmap every
        * single time an invalidate call is made.
        */
        private BitmapDrawable GetAndAddHoverView(View v) {

            int w = v.Width;
            int h = v.Height;
            int top = v.Top;
            int left = v.Left;

            Bitmap b = GetBitmapWithBorder(v);

            BitmapDrawable drawable = new BitmapDrawable(Resources, b);

            _hoverCellOriginalBounds = new Rect(left, top, left + w, top + h);
            _hoverCellCurrentBounds = new Rect(_hoverCellOriginalBounds);

            drawable.Bounds = _hoverCellCurrentBounds;

            return drawable;
        }

        /** Draws a black border over the screenshot of the view passed in. */
        private Bitmap GetBitmapWithBorder(View v) {
            Bitmap bitmap = GetBitmapFromView(v);
            Canvas can = new Canvas(bitmap);

            Rect rect = new Rect(0, 0, bitmap.Width, bitmap.Height);

            Paint paint = new Paint();
            paint.SetStyle(Paint.Style.Stroke);
            paint.StrokeWidth = LINE_THICKNESS;
            paint.Color = Color.Black;

            can.DrawBitmap(bitmap, 0, 0, null);
            can.DrawRect(rect, paint);

            return bitmap;
        }

        /** Returns a bitmap showing a screenshot of the view passed in. */
        private Bitmap GetBitmapFromView(View v) {
            Bitmap bitmap = Bitmap.CreateBitmap(v.Width, v.Height, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(bitmap);
            v.Draw(canvas);
            return bitmap;
        }

        /**
         * Stores a reference to the views above and below the item currently
         * corresponding to the hover cell. It is important to note that if this
         * item is either at the top or bottom of the list, mAboveItemId or mBelowItemId
         * may be invalid.
         */
        private void UpdateNeighborViewsForID(long itemID) {
            int position = GetPositionForID(itemID);
            StableArrayAdapter adapter = ((StableArrayAdapter) Adapter);
            _aboveItemId = adapter.GetItemId(position - 1);
            _belowItemId = adapter.GetItemId(position + 1);
        }

        /** Retrieves the view in the list corresponding to itemID */
        public View GetViewForID(long itemID) {
            int firstVisiblePosition = FirstVisiblePosition;
            StableArrayAdapter adapter = ((StableArrayAdapter) Adapter);
            for (int i = 0; i < ChildCount; i++) {
                View v = GetChildAt(i);
                int position = firstVisiblePosition + i;
                long id = adapter.GetItemId(position);
                if (id == itemID) {
                    return v;
                }
            }
            return null;
        }

        /** Retrieves the position in the list corresponding to itemID */
        public int GetPositionForID(long itemID) {
            View v = GetViewForID(itemID);
            if (v == null) {
                return -1;
            } else {
                return GetPositionForView(v);
            }
        }

        /**
        *  dispatchDraw gets invoked when all the child views are about to be drawn.
        *  By overriding this method, the hover cell (BitmapDrawable) can be drawn
        *  over the listview's items whenever the listview is redrawn.
        */
        protected override void DispatchDraw(Canvas canvas) {
            base.DispatchDraw(canvas);
            if (_hoverCell != null) {
                _hoverCell.Draw(canvas);
            }
        }

        public override bool OnTouchEvent(MotionEvent e) {

            switch(e.Action & MotionEventActions.Mask) {
                case MotionEventActions.Down:
                    _downX = (int) e.XPrecision;
                    _downY = (int) e.YPrecision;
                    _activePointerId = e.GetPointerId(0);
                    break;
                case MotionEventActions.Move:
                    if(_activePointerId == INVALID_POINTER_ID) { break; }

                    var pointerIndex = e.FindPointerIndex(_activePointerId);

                    _lastEventY = (int) e.GetY(pointerIndex);
                    var deltaY = _lastEventY - _downY;

                    if (_cellIsMobile) {
                        _hoverCellCurrentBounds.OffsetTo(_hoverCellOriginalBounds.Left,
                        _hoverCellOriginalBounds.Top + deltaY + _totalOffset);
                        _hoverCell.Bounds = _hoverCellCurrentBounds;
                        Invalidate();

                        HandleCellSwitch();

                        _isMobileScrolling = false;
                        HandleMobileCellScroll();

                        return false;
                    }
                    break;
                case MotionEventActions.Up:
                    TouchEventsEnded(); break;
                case MotionEventActions.Cancel:
                    TouchEventsCancelled(); break;
                case MotionEventActions.PointerUp:
                    /* If a multitouch event took place and the original touch dictating
                    * the movement of the hover cell has ended, then the dragging event
                    * ends and the hover cell is animated to its corresponding position
                    * in the listview. */
                    pointerIndex = (int) (e.Action & MotionEventActions.PointerIndexMask) >>
                       (int) MotionEventActions.PointerIndexShift;
                    var pointerId = e.GetPointerId(pointerIndex);
                    if(pointerId == pointerIndex) {
                        TouchEventsEnded();
                    }
                    break;
                default: break;

            }

            return base.OnTouchEvent(e);
        }


        /**
        * This method determines whether the hover cell has been shifted far enough
        * to invoke a cell swap. If so, then the respective cell swap candidate is
        * determined and the data set is changed. Upon posting a notification of the
        * data set change, a layout is invoked to place the cells in the right place.
        * Using a ViewTreeObserver and a corresponding OnPreDrawListener, we can
        * offset the cell being swapped to where it previously was and then animate it to
        * its new position.
        */
        private void HandleCellSwitch() {
            int deltaY = _lastEventY - _downY;
            int deltaYTotal = _hoverCellOriginalBounds.Top + _totalOffset + deltaY;

            View belowView = GetViewForID(_belowItemId);
            View mobileView = GetViewForID(_mobileItemId);
            View aboveView = GetViewForID(_aboveItemId);

            bool isBelow = (belowView != null) && (deltaYTotal > belowView.Top);
            bool isAbove = (aboveView != null) && (deltaYTotal < aboveView.Top);

            if (isBelow || isAbove) {

                long switchItemID = isBelow ? _belowItemId : _aboveItemId;
                View switchView = isBelow ? belowView : aboveView;
                int originalItem = GetPositionForView(mobileView);

                if (switchView == null) {
                    UpdateNeighborViewsForID(_mobileItemId);
                    return;
                }

                SwapElements(_viewList, originalItem, GetPositionForView(switchView));

                ((BaseAdapter) Adapter).NotifyDataSetChanged();

                _downY = _lastEventY;

                int switchViewStartTop = switchView.Top;

                mobileView.Visibility = ViewStates.Visible;
                switchView.Visibility = ViewStates.Invisible;

                UpdateNeighborViewsForID(_mobileItemId);

                ViewTreeObserver observer = ViewTreeObserver;
                observer.PreDraw += (s, e) => {
                    View newSwitchView = GetViewForID(switchItemID);

                    _totalOffset += deltaY;

                    int switchViewNewTop = newSwitchView.Top;
                    int delta = switchViewStartTop - switchViewNewTop;

                    newSwitchView.TranslationY = delta;

                    ObjectAnimator animator = ObjectAnimator.OfFloat(newSwitchView, Y, 0);
                    animator.SetDuration(MOVE_DURATION);
                    animator.Start(); 

                };

                 
             }
         }

        private void SwapElements(List<View> list, int indexOne, int indexTwo) {
            View temp = list.ElementAt(indexOne);
            list.Insert(indexOne, list.ElementAt(indexTwo));
            list.Insert(indexTwo, temp);
        }

        /**
        * Resets all the appropriate fields to a default state while also animating
        * the hover cell back to its correct location.
        */
        private void TouchEventsEnded() {
            View mobileView = GetViewForID(_mobileItemId);
            if (_cellIsMobile || _isWaitingForScrollFinish) {
                _cellIsMobile = false;
                _isWaitingForScrollFinish = false;
                _isMobileScrolling = false;
                _activePointerId = INVALID_POINTER_ID;

                // If the autoscroller has not completed scrolling, we need to wait for it to
                // finish in order to determine the final location of where the hover cell
                // should be animated to.
                if (_scrollState != (int) ScrollState.Idle) {
                    _isWaitingForScrollFinish = true;
                    return;
                }

                _hoverCellCurrentBounds.OffsetTo(_hoverCellOriginalBounds.Left, mobileView.Top);

                ObjectAnimator hoverViewAnimator = ObjectAnimator.OfObject(_hoverCell, "bounds",
                        this, _hoverCellCurrentBounds);

                hoverViewAnimator.Update += (s, e) => { Invalidate(); };

                hoverViewAnimator.AnimationStart += (s, e) => { Enabled = false; };

                hoverViewAnimator.AnimationEnd += (s, e) => {
                    _aboveItemId = INVALID_ID;
                    _mobileItemId = INVALID_ID;
                    _belowItemId = INVALID_ID;
                    mobileView.Visibility = ViewStates.Visible;
                    _hoverCell = null;
                    Enabled = true;
                    Invalidate();
                };

                hoverViewAnimator.Start();

            } else {
                TouchEventsCancelled();
            }
        }

        /**
         * Resets all the appropriate fields to a default state.
         */
        private void TouchEventsCancelled() {
            View mobileView = GetViewForID(_mobileItemId);
            if (_cellIsMobile) {
                _aboveItemId = INVALID_ID;
                _mobileItemId = INVALID_ID;
                _belowItemId = INVALID_ID;
                mobileView.Visibility = ViewStates.Visible; 
                _hoverCell = null;
                Invalidate();
            }
            _cellIsMobile = false;
            _isMobileScrolling = false;
            _activePointerId = INVALID_POINTER_ID;
        }

        /**
         * Evaluate is used to animate the BitmapDrawable back to its
         * final location when the user lifts his finger by modifying the
         * BitmapDrawable's bounds.
         */
        public Java.Lang.Object Evaluate(float fraction, Java.Lang.Object startValue, Java.Lang.Object endValue) {
            Rect start = (Rect) startValue;
            Rect end = (Rect) endValue;

            return new Rect(interpolate(start.Left, end.Left, fraction),
                    interpolate(start.Top, end.Top, fraction),
                    interpolate(start.Right, end.Right, fraction),
                    interpolate(start.Bottom, end.Bottom, fraction));
        }

        public int interpolate(int start, int end, float fraction) {
            return (int) (start + fraction * (end - start));
        }

        /**
         *  Determines whether this listview is in a scrolling state invoked
         *  by the fact that the hover cell is out of the bounds of the listview;
         */
        private void HandleMobileCellScroll() {
            _isMobileScrolling = HandleMobileCellScroll(_hoverCellCurrentBounds);
        }

        /**
        * This method is in charge of determining if the hover cell is above
        * or below the bounds of the listview. If so, the listview does an appropriate
        * upward or downward smooth scroll so as to reveal new items.
        */
        public bool HandleMobileCellScroll(Rect r) {
            int offset = ComputeVerticalScrollOffset();
            int height = Height;
            int extent = ComputeVerticalScrollExtent();
            int range = ComputeVerticalScrollRange();
            int hoverViewTop = r.Top;
            int hoverHeight = r.Height();

            if (hoverViewTop <= 0 && offset > 0) {
                SmoothScrollBy(-_smoothScrollAmountAtEdge, 0);
                return true;
            }

            if (hoverViewTop + hoverHeight >= height && (offset + extent) < range) {
                SmoothScrollBy(_smoothScrollAmountAtEdge, 0);
                return true;
            }

            return false;
        }

        public void setViewList(List<View> viewList) {
            _viewList = viewList;
        }

        public void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount) {
            _currentFirstVisibleItem = firstVisibleItem;
            _currentVisibleItemCount = visibleItemCount;

            _previousFirstVisibleItem = (_previousFirstVisibleItem == -1) ? _currentFirstVisibleItem
                    : _previousFirstVisibleItem;
            _previousVisibleItemCount = (_previousVisibleItemCount == -1) ? _currentVisibleItemCount
                    : _previousVisibleItemCount;

            CheckAndHandleFirstVisibleCellChange();
            CheckAndHandleLastVisibleCellChange();

            _previousFirstVisibleItem = _currentFirstVisibleItem;
            _previousVisibleItemCount = _currentVisibleItemCount;
        }

        public void OnScrollStateChanged(AbsListView view, [GeneratedEnum] ScrollState scrollState) {
            _currentScrollState = (int) scrollState;
            _scrollState = (int) scrollState;
            IsScrollCompleted();
        }


        /**
         * This method is in charge of invoking 1 of 2 actions. Firstly, if the listview
         * is in a state of scrolling invoked by the hover cell being outside the bounds
         * of the listview, then this scrolling event is continued. Secondly, if the hover
         * cell has already been released, this invokes the animation for the hover cell
         * to return to its correct position after the listview has entered an idle scroll
         * state.
         */
        private void IsScrollCompleted() {
            if (_currentVisibleItemCount > 0 && _currentScrollState == (int) ScrollState.Idle) {
                if (_cellIsMobile && _isMobileScrolling) {
                    HandleMobileCellScroll();
                } else if (_isWaitingForScrollFinish) {
                    TouchEventsEnded();
                }
            }
        }

        /**
         * Determines if the listview scrolled up enough to reveal a new cell at the
         * top of the list. If so, then the appropriate parameters are updated.
         */
        public void CheckAndHandleFirstVisibleCellChange() {
            if (_currentFirstVisibleItem != _previousFirstVisibleItem) {
                if (_cellIsMobile && _mobileItemId != INVALID_ID) {
                    UpdateNeighborViewsForID(_mobileItemId);
                    HandleCellSwitch();
                }
            }
        }

        /**
        * Determines if the listview scrolled down enough to reveal a new cell at the
        * bottom of the list. If so, then the appropriate parameters are updated.
        */
        public void CheckAndHandleLastVisibleCellChange() {
            int currentLastVisibleItem = _currentFirstVisibleItem + _currentVisibleItemCount;
            int previousLastVisibleItem = _previousFirstVisibleItem + _previousVisibleItemCount;
            if (currentLastVisibleItem != previousLastVisibleItem) {
                if (_cellIsMobile && _mobileItemId != INVALID_ID) {
                    UpdateNeighborViewsForID(_mobileItemId);
                    HandleCellSwitch();
                }
            }
        }

    }
}
