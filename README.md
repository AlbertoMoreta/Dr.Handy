<h1 align="center">
  <br>
  <a href="https://github.com/AlbertoMoreta/Dr.Handy">
  <img src="ReadMeAssets/Logo.png" alt="Markdownify" width="150"></a>
  <br>
  Dr. Handy
  <br>
</h1>

[![Build status](https://build.mobile.azure.com/v0.1/apps/6024c019-acf3-43b1-80c2-d1fdcd19883f/branches/android-dev/badge)](https://mobile.azure.com)

<h4 align="center">
  Dr. Handy is a mobile app that works as a health center, allowing you to have a lot of medical/health kind of tools in just one app.
</h4>

<br>
<br>

<p align="center">
<img src="ReadMeAssets/Screenshot1.png" width="150" style="margin-right:5px; border: 1px solid #ccc;"/>
<img src="ReadMeAssets/Screenshot2.png" width="150" style="margin-right:5px; border: 1px solid #ccc;" />
<img src="ReadMeAssets/Screenshot3.png" width="150" style="margin-right:5px; border: 1px solid #ccc;" />
<img src="ReadMeAssets/Screenshot4.png" width="150" style="margin-right:5px; border: 1px solid #ccc;" />
<img src="ReadMeAssets/Screenshot5.png" width="150" style="margin-right:5px; border: 1px solid #ccc;" />
</p>

<br>
<br>

## Contributing

Use the following image for reference: 

<h1 align="center">
  <br>
  <a href="https://github.com/AlbertoMoreta/Dr.Handy">
  <img src="ReadMeAssets/GitBranchStructure.png" alt="Markdownify" width="70%"></a>
</h1>

1. Fork it!
2. Create your shared dev branch: `git checkout -b shared-{module-shortname} shared-dev`.  In this branch, you can add the shared functionality of your module for Android, iOS and Windows Phone projects.
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin shared-{module-shortname}`
5. Create your specific dev branch: `git checkout -b {platform-name}-{module-shortname} {platform-name}-healthmodules`.  In this branch, you can add the specific functionality of your module for the indicated {platform-name}: Android, iOS or Windows-Phone.
6. Merge the contents from the shared-dev branch to the specific-dev branch: `git {platform-name}-{module-shortname} shared-{module-shortname}`
7. Commit your changes: `git commit -am 'Add some feature'`
8. Push to the branch: `git push origin {platform-name}-{module-shortname}`
9. Submit a pull request :D

## Release History

* 0.1.0
    * First release
    * CHANGE: Step Counter, Color Blindness Test & Sintrom.
* 0.0.1
    * Work in progress

## Built With

* [Xamarin](https://www.xamarin.com/) - The mobile app framework used
* [Firebase](https://firebase.google.com/) - Google Authentication

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

* [Philipp Jahoda - MPAndroidCharts](https://github.com/PhilJay/MPAndroidChart): For the charts used in the app.
