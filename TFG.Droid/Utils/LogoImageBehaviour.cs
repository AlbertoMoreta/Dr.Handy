using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace TFG.Droid.Utils {
    [Register("CustomBehaviors.LogoImageBehaviour")]
    public class LogoImageBehaviour : CoordinatorLayout.Behavior {

        private Context _context;

        private float _startToolbarPosition;
        private float _changeBehaviorPoint;
        private int _startXPosition;
        private int _finalXPosition;
        private int _startYPosition;
        private int _finalYPosition;
        private int _startHeight;
        private float _customFinalHeight;

        public LogoImageBehaviour(Context context, IAttributeSet attrs)  { _context = context;} 
        public LogoImageBehaviour(Context context) { _context = context; }


        public override bool LayoutDependsOn(CoordinatorLayout parent, Java.Lang.Object child, View dependency){
            var type = dependency.GetType();
            var value = dependency is Android.Support.V7.Widget.Toolbar;
            return value;
        }

        public override bool OnDependentViewChanged(CoordinatorLayout parent, Java.Lang.Object child, View dependency) {

            var image = child.JavaCast <ImageView>();  
            InitProperties(image, dependency);


            var maxScrollDistance = (int) _startToolbarPosition;
            var expandedPercentageFactor = maxScrollDistance == 0? 0: dependency.GetY() / maxScrollDistance;

            if (expandedPercentageFactor < _changeBehaviorPoint) {
                var heightFactor = (_changeBehaviorPoint - expandedPercentageFactor) / _changeBehaviorPoint;

                var distanceXToSubtract = ((_startXPosition - _finalXPosition) * heightFactor) + (image.Height/2);
                var distanceYToSubtract = ((_startYPosition - _finalYPosition) * (1f - expandedPercentageFactor)) + (image.Height/2);

                image.SetX(_startXPosition - distanceXToSubtract);
                image.SetY(_startYPosition - distanceYToSubtract); 

                float heightToSubtract = ((_startHeight - _customFinalHeight) * heightFactor);

                CoordinatorLayout.LayoutParams lp = (CoordinatorLayout.LayoutParams) image.LayoutParameters;
                lp.Width = (int) (_startHeight - heightToSubtract);
                lp.Height = (int) (_startHeight - heightToSubtract);
                image.LayoutParameters = lp;
            } else {
                float distanceYToSubtract = ((_startYPosition - _finalYPosition) * (1f - expandedPercentageFactor)) + (_startHeight/2);

                image.SetX(_startXPosition - image.Width/2);
                image.SetY(_startYPosition - distanceYToSubtract);

                CoordinatorLayout.LayoutParams lp = (CoordinatorLayout.LayoutParams) image.LayoutParameters;
                lp.Width = (int) (_startHeight);
                lp.Height = (int) (_startHeight);
                image.LayoutParameters = lp; 
            }
            return true;
        }

        private void InitProperties(ImageView image, View dependency) {

            if (_customFinalHeight == 0) { _customFinalHeight = _context.Resources.GetDimensionPixelOffset(Resource.Dimension.image_final_size); }
            if (_startYPosition == 0) { _startYPosition = (int) dependency.GetY(); }
            if (_finalYPosition == 0) { _finalYPosition = (int) (_customFinalHeight + dependency.Height) /2; }
            if (_startHeight == 0) { _startHeight = image.Height; }
            if (_startXPosition == 0) { _startXPosition = (int) (image.GetX() + (image.Width / 2)); }
            if (_finalXPosition == 0) {
                _finalXPosition =
                    _context.Resources.GetDimensionPixelOffset(
                        Resource.Dimension.abc_action_bar_content_inset_material) + ((int) _customFinalHeight/2) - (int) _customFinalHeight;
            }
            if (_startToolbarPosition == 0) { _startToolbarPosition = dependency.GetY(); }
            if (_changeBehaviorPoint == 0) {
                _changeBehaviorPoint = (image.Height + 2f*_customFinalHeight)/(2f*(_startYPosition - _finalYPosition));
            }

        }
    }
}