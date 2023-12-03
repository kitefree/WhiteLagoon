$(document).ready(function () {
    loadCustomerBookingPieChart();
});
function loadCustomerBookingPieChart() {
    $(".chart-spinner").show();    
    $.ajax({
        url: "/Dashboard/GetBookingPieChartData",
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $(".chart-spinner").hide();


            loadPieChart('customerBookingsPieChart', data);


        }
    });
}


function loadPieChart(id, data) {
    var chartColors = getChartColorsArray(id);

    var options = {
        series: data.series,
        labels: data.labels,
        colors: chartColors,
        chart: {
            type: 'pie',
            width:380
        },
        stroke: {
            show:false
        },
        legend: {
            position: 'bottom',
            horizontalAlign: 'center',
            labels: {
                colors: '#fff',
                useSeriesColor:true
            }
        }
    };
    var chart = new ApexCharts(document.querySelector(`#${id}`), options);
    chart.render();

}