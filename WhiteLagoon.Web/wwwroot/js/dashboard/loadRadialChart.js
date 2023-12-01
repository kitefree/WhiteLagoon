
function loadRadialChart(id, data) {
    var chartColors = getChartColorsArray(id);
    var options = {
        fill: {
            colors:chartColors
        },
        chart: {
            height: 280,
            type: "radialBar"
        },
        series: data.series,
        plotOptions: {
            radialBar: {
                hollow: {
                    margin: 15,
                    size: "70%"
                },
                dataLabels: {
                    showOn: "always",
                    name: {
                        offsetY: -10,
                        show: true,
                        color: "#888",
                        fontSize: "13px"
                    },
                    value: {
                        color: "#111",
                        fontSize: "30px",
                        show: true
                    }
                }
            }
        },
        stroke: {
            lineCap: "round",
        }
    };

    var chart = new ApexCharts(document.querySelector(`#${id}`), options);

    chart.render();



}


function getChartColorsArray(id) {
    if (document.getElementById(id) !== null) {
        var colors = document.getElementById(id).getAttribute("data-colors");
        if (colors) {
            colors = JSON.parse(colors);
            return colors.map(function (value) {
            });
            var newValue = value.replace(" ", "");
            if (newValue.indexOf(",") === -1) {
                var color = getComputedStyle
                if (color) return color;
                else return newValue;;
                (document.documentElement).getPropertyValue(newValue);
            }
        }
    }
}