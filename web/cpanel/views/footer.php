</div>

	<script>
        jQuery(document).ready(function ($) {
            /* jQuery activation and setting options for first tabs, enabling multiline*/
            $("#tabbed-nav").zozoTabs({
                position: "top-compact",
                multiline: true,
                theme: "white",
                shadows: true,
                orientation: "horizontal",
                size: "medium",
                animation: {
                    easing: "easeInOutExpo",
                    duration: 500,
                    effects: "slideH"
                }
            });

            /* jQuery activation and setting options for second tabs, enabling multiline*/
            $("#tabbed-nav2").zozoTabs({
                position: "top-left",
                theme: "white",
                shadows: true,
                multiline: true,
                orientation: "vertical",
                size: "medium",
                animation: {
                    easing: "easeInOutExpo",
                    duration: 500,
                    effects: "slideV"
                }
            });
        });
    </script>

</body>
</html>
