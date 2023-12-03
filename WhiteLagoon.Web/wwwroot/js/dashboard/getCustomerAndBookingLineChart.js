$(document).ready(function () {
    loadCustomerBookingLineChart();
});
function loadCustomerBookingLineChart() {
    $(".chart-spinner").show();    
    $.ajax({
        url: "/Dashboard/GetMemberAndBookingLineChartData",
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $(".chart-spinner").hide();
            loadLineChart('newMembersAndBookingsLineChart', data);
        }
    });
}


function loadLineChart(id, data) {
    var chartColors = getChartColorsArray(id);

    var options = {

        series: data.series,
        colors: chartColors,
        chart: {
            height:350,
            type: 'line',            
        },
        stroke: {
            curve: 'smooth',
            width: 2
        },
        markers: {
            size: 3,
            strokeWidth: 0,
            hover: {
                size: 7
            }
        },
        xaxis: {
            categories: data.categories,
            labels: {
                style: {
                    colors: "#ddd",
                }
            }
        },
        yaxis: {
            labels: {
                style: {
                    colors:"#fff",
                }
            }
        },
        tooltip: {
            theme:'dark'
        }

    };
    var chart = new ApexCharts(document.querySelector(`#${id}`), options);
    chart.render();

}