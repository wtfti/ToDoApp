var gulp = require('gulp')
var uglify = require('gulp-uglify')
var rename = require('gulp-rename')
var minifyCSS = require('gulp-minify-css')

gulp.task('compressjs', function () {
  gulp.src('libs/*.js')
    .pipe(uglify())
	.pipe(rename({ suffix: '.min' }))
    .pipe(gulp.dest('./build/js/'))
})

gulp.task('compressCss', function () {
	gulp.src('content/*.css')
		.pipe(minifyCSS())
		.pipe(rename({ suffix: '.min'}))
		.pipe(gulp.dest('./build/css/'))
})