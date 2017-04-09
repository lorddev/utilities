/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. https://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var bump = require("gulp-bump")

gulp.task('default',
    function() {
        // place code for your default task here
    });

gulp.task("bump",
    function() {
        gulp.src("./Devlord.Utilities.csproj")
            .pipe(bump())
            .pipe(gulp.dest("./"));
    });