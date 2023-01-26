<!DOCTYPE html>
<html>
<head>
	<title>fancyBox - Fancy jQuery Lightbox Alternative | Demonstration</title>
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

	<!-- Add jQuery library -->
	<script type="text/javascript" src="lib/jquery-1.10.1.min.js"></script>

	<!-- Add mousewheel plugin (this is optional) -->
	<script type="text/javascript" src="lib/jquery.mousewheel-3.0.6.pack.js"></script>

	<!-- Add fancyBox main JS and CSS files -->
	<script type="text/javascript" src="source/jquery.fancybox.js?v=2.1.5"></script>
	<link rel="stylesheet" type="text/css" href="source/jquery.fancybox.css?v=2.1.5" media="screen" />

	
	<script type="text/javascript">
		$(document).ready(function() {
			/*
			 *  Simple image gallery. Uses default settings
			 */

			$('.fancybox').fancybox();

			/*
			 *  Different effects
			 */

		


		});
	</script>
	<style type="text/css">
		.fancybox-custom .fancybox-skin {
			box-shadow: 0 0 50px #222;
		}

		body {
			max-width: 700px;
			margin: 0 auto;
		}
	</style>
</head>
<body>
	<h1>Image Gallery</h1>

	<p>
		<a class="fancybox" href="a.jpg" data-fancybox-group="gallery" title="A network for change"><img src="b.jpg" alt="" /></a>

		<a class="fancybox" href="2_b.jpg" data-fancybox-group="gallery" title="A network for change"><img src="2_s.jpg" alt="" /></a>

		<a class="fancybox" href="3_b.jpg" data-fancybox-group="gallery" title="A network for change"><img src="3_s.jpg" alt="" /></a>

		<a class="fancybox" href="4_b.jpg" data-fancybox-group="gallery" title="A network for change"><img src="4_s.jpg" alt="" /></a>
	</p>

</body>
</html>