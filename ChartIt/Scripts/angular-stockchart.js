'use strict';
// 1.0.4

angular.module('amCharts', []).directive('stockChart', ['$q', function ($q) {
  return {
    restrict: 'E',
    replace: true,
    scope: {
      options: '=',
      chart: '=?',
      height: '@',
      width: '@',
      id: '@'
    },
    template: '<div class="amchart"></div>',
    link: function ($scope, $el) {


      var id = getIdForUseInAmCharts();
      $el.attr('id', id);
      var chart;
      $scope.chart = chart;

      // allow $scope.options to be a promise
        $q.when($scope.options).then(function(options){
        // we can't render a chart without any data
           
            if (options.data || (options.dataSets && options.dataSets.length > 0))
            {
                var renderChart = function (amChartOptions) {
                    var o = amChartOptions || options;

                    // set height and width
                    var height = $scope.height || '100%';
                    var width = $scope.width || '100%';

                    $el.css({
                        'height': height,
                        'width': width
                    });

                    // instantiate new chart object
                    if (o.type === 'xy') {
                        chart = o.theme ? new AmCharts.AmXYChart(AmCharts.themes[o.theme]) : new AmCharts.AmXYChart();
                    } else if (o.type === 'pie') {
                        chart = o.theme ? new AmCharts.AmPieChart(AmCharts.themes[o.theme]) : new AmCharts.AmPieChart();
                    } else if (o.type === 'funnel') {
                        chart = o.theme ? new AmCharts.AmFunnelChart(AmCharts.themes[o.theme]) : new AmCharts.AmFunnelChart();
                    } else if (o.type === 'radar') {
                        chart = o.theme ? new AmCharts.AmRadarChart(AmCharts.themes[o.theme]) : new AmCharts.AmRadarChart();
                    } else if (o.type === 'gauge') {
                        chart = o.theme ? new AmCharts.AmAngularGauge(AmCharts.themes[o.theme]) : new AmCharts.AmAngularGauge();
                    } else if (o.type === 'stock') {
                        chart = o.theme ? new AmCharts.AmStockChart(AmCharts.themes[o.theme]) : new AmCharts.AmStockChart();
                    } else {
                        chart = o.theme ? new AmCharts.AmSerialChart(AmCharts.themes[o.theme]) : new AmCharts.AmSerialChart();
                    }

                   

                    $scope.$watch('o.dataSets[0].dataProvider', function () {

                        init(o, o.dataSets);
                    });

                }; //renderchart
 
              // Render the chart
                renderChart();

                function init(o, datasets) {
                    var classDataSets = [];

                    for (var i = 0; i < datasets.length; i++) { 
                        var set = new AmCharts.DataSet();
                        set.title = datasets[i].title;
                        set.fieldMappings = datasets[i].fieldMappings;
                        set.dataProvider = datasets[i].dataProvider;
                        set.categoryField = datasets[i].categoryField;

                        classDataSets.push(set);
                    }

                    chart.dataSets = classDataSets;
                    
                    chart.startDuration = 0.5; // default animation length, because everyone loves a little pizazz

                    // AutoMargin is on by default, but the default 20px all around seems to create unnecessary white space around the control
                    chart.autoMargins = true;
                    chart.marginTop = 0;
                    chart.marginLeft = 0;
                    chart.marginBottom = 0;
                    chart.marginRight = 0;

                    // modify default creditsPosition
                    chart.creditsPosition = 'top-right';

                    if (o.categoryAxesSettings) {

                        var axesSettings = new AmCharts.CategoryAxesSettings();
                        axesSettings.minPeriod = o.categoryAxesSettings.minPeriod;
                        chart.categoryAxesSettings = axesSettings;
                    }

                    function generateGraphProperties(data) {
                        // Assign Category Axis Properties
                        if (o.categoryAxis) {
                            var categoryAxis = chart.categoryAxis;

                            if (categoryAxis) {
                                /* if we need to create any default values, we should assign them here */
                                categoryAxis.parseDates = true;

                                var keys = Object.keys(o.categoryAxis);
                                for (var i = 0; i < keys.length; i++) {
                                    if (!angular.isObject(o.categoryAxis[keys[i]]) || angular.isArray(o.categoryAxis[keys[i]])) {
                                        categoryAxis[keys[i]] = o.categoryAxis[keys[i]];
                                    } else {
                                        console.log('Stripped categoryAxis obj ' + keys[i]);
                                    }
                                }
                                chart.categoryAxis = categoryAxis;
                            }
                        }

                        // Create value axis
                       
                        /* if we need to create any default values, we should assign them here */

                        var addValueAxis = function (a) {
                            var valueAxis = new AmCharts.ValueAxis();

                            var keys = Object.keys(a);
                            for (var i = 0; i < keys.length; i++) {
                                valueAxis[keys[i]] = a[keys[i]];
                            }
                            chart.addValueAxis(valueAxis);
                        };

                        if (o.valueAxes && o.valueAxes.length > 0) {
                            for (var i = 0; i < o.valueAxes.length; i++) {
                                addValueAxis(o.valueAxes[i]);
                            }
                        }

                        //reusable function to create graph
                        var addGraph = function (g) {
                            var graph = new AmCharts.AmGraph();
                            /** set some default values that amCharts doesnt provide **/
                            // if a category field is not specified, attempt to use the second field from an object in the array as a default value
                            if (g && o.data && o.data.length > 0) {
                                graph.valueField = g.valueField || Object.keys(o.data[0])[1];
                            }
                            graph.balloonText = '<span style="font-size:14px">[[category]]: <b>[[value]]</b></span>';
                            if (g) {
                                var keys = Object.keys(g);
                                // iterate over all of the properties in the graph object and apply them to the new AmGraph
                                for (var i = 0; i < keys.length; i++) {
                                    graph[keys[i]] = g[keys[i]];
                                }
                            }
                            chart.addGraph(graph);
                        };

                        if (o.type == 'gauge') {
                            if (o.axes && o.axes.length > 0) {
                                for (var i = 0; i < o.axes.length; i++) {
                                    var axis = new AmCharts.GaugeAxis();
                                    Object.assign(axis, o.axes[i]);
                                    chart.addAxis(axis);
                                }
                            }
                            if (o.arrows && o.arrows.length > 0) {
                                for (var i = 0; i < o.arrows.length; i++) {
                                    var arrow = new AmCharts.GaugeArrow();
                                    Object.assign(arrow, o.arrows[i]);
                                    chart.addArrow(arrow);
                                }
                            }
                        }
                        else if (o.type === 'stock') {
                            var panelArr = [];

                           
                            for (var i = 0; i < o.panels.length; i++)
                            {
                                var panel = new AmCharts.StockPanel();
                                var graphs = [];

                                panel.title = o.panels[i].title;
                                panel.showCategoryAxis = o.panels[i].showCategoryAxis;
                                panel.percentHeight = o.panels[i].percentHeight;
                                 
                                panel.categoryAxis = o.panels[i].categoryAxis;
                             
                                if (o.panels[i].valueAxes && o.panels[i].valueAxes.length > 0) {
                                    for (var j = 0; j < o.panels[i].valueAxes.length; j++) {
                                        var valueAxis = new AmCharts.ValueAxis();

                                        var keys = Object.keys(o.panels[i].valueAxes[j]);
                                        for (var k = 0; k < keys.length; k++) {
                                            valueAxis[keys[k]] = o.panels[i].valueAxes[j][keys[k]];
                                        }
                                        panel.addValueAxis(valueAxis);
                                    }
                                }
                             
                                for (var j = 0; j < o.panels[i].stockGraphs.length; j++)
                                {
                                    var graphConfig = o.panels[i].stockGraphs[j];
                                    var graph = new AmCharts.StockGraph();

                                    var keys = Object.keys(graphConfig);
                                    for (var k = 0; k < keys.length; k++) {
                                        graph[keys[k]] = graphConfig[keys[k]];
                                    }
                                  
                                    panel.addStockGraph(graph); 
                                }

                                 
                                if (o.panels[i].stockLegend) {
                                    var legend = new AmCharts.StockLegend();
                                    var keys = Object.keys(o.panels[i].stockLegend);
                                    for (var j = 0; j < keys.length; j++) {
                                        legend[keys[j]] = o.panels[i].stockLegend[keys[j]];
                                    }
                                    panel.stockLegend  = legend;
                                }
                              
                                 
                                panelArr.push(panel); 
                            }
                             
                            chart.panels = panelArr;

                            if (o.chartScrollbarSettings) {
                                var scrollbarSettings = new AmCharts.ChartScrollbarSettings();
                                var keys = Object.keys(o.chartScrollbarSettings);
                                for (var j = 0; j < keys.length; j++) {
                                    scrollbarSettings[keys[j]] = o.chartScrollbarSettings[keys[j]];
                                }
                                chart.chartScrollbarSettings = scrollbarSettings;
                            }
                          
                            if (o.chartCursorSettings) {
                                var cursorSettings = new AmCharts.ChartCursorSettings();
                                var keys = Object.keys(o.chartCursorSettings);
                                for (var j = 0; j < keys.length; j++) {
                                    cursorSettings[keys[j]] = o.chartCursorSettings[keys[j]];
                                }
                                chart.chartCursorSettings = cursorSettings;
                            }

                            if (o.dataSetSelector && o.dataSets.length > 1) {
                                var dataSetSelectorSettings = new AmCharts.DataSetSelector();
                                var keys = Object.keys(o.dataSetSelector);
                                for (var j = 0; j < keys.length; j++) {
                                    dataSetSelectorSettings[keys[j]] = o.dataSetSelector[keys[j]];
                                }
                                chart.dataSetSelector = dataSetSelectorSettings;
                            }
                            
 

                        }
                        else {
                            // create the graphs
                            if (o.graphs && o.graphs.length > 0) {
                                for (var i = 0; i < o.graphs.length; i++) {
                                    addGraph(o.graphs[i]);
                                }
                            } else {
                                addGraph();
                            }
                        }

                        if (o.type === 'gantt' || o.type === 'serial' || o.type === 'xy') {
                            var chartCursor = new AmCharts.ChartCursor();
                            if (o.chartCursor) {
                                var keys = Object.keys(o.chartCursor);
                                for (var i = 0; i < keys.length; i++) {
                                    if (typeof o.chartCursor[keys[i]] !== 'object') {
                                        chartCursor[keys[i]] = o.chartCursor[keys[i]];
                                    }
                                }
                            }
                            chart.addChartCursor(chartCursor);
                        }

                        if (o.chartScrollbar) {
                            var scrollbar = new AmCharts.ChartScrollbar();
                            var keys = Object.keys(o.chartScrollbar);
                            for (var i = 0; i < keys.length; i++) {
                                scrollbar[keys[i]] = o.chartScrollbar[keys[i]];
                            }
                            chart.chartScrollbar = scrollbar;
                        }

                        if (o.balloon) {
                            chart.balloon = o.balloon;
                        }
                    }

                    function generatePieProperties() {
                        if (o.balloon) {
                            chart.balloon = o.balloon;
                        }
                        if (o.balloonFunction) {
                            chart.balloonFunction = o.balloonFunction;
                        }
                    }

                    if (o.legend) {
                        var legend = new AmCharts.AmLegend();
                        var keys = Object.keys(o.legend);
                        for (var i = 0; i < keys.length; i++) {
                            legend[keys[i]] = o.legend[keys[i]];
                        }
                        chart.legend = legend;
                    }

                    if (o.type === 'pie') {
                        generatePieProperties();
                    } else {
                        generateGraphProperties();
                    }

                    if (o.titles) {
                        for (var i = 0; i < o.titles.length; i++) {
                            var title = o.titles[i];
                            chart.addTitle(title.text, title.size, title.color, title.alpha, title.bold);
                        };
                    }

                    if (o.allLabels) {
                        chart.allLabels = o.allLabels;
                    }

                    if (o.labelFunction) {
                        chart.labelFunction = o.labelFunction;
                    }

                    if (o.export) {
                        chart.amExport = o.export;
                        chart.export = o.export;
                    }

                    if (o.responsive) {
                        chart.responsive = o.responsive;
                    }

                    if (o.colors) {
                        chart.colors = o.colors;
                    }

                    if (o.defs) {
                        chart.defs = o.defs;
                    }

                    if (o.listeners) {
                        for (var i = 0; i < o.listeners.length; i++) {
                            chart.addListener(o.listeners[i].event, o.listeners[i].method);
                        }
                    }

                    var addEventListeners = function (obj, chartObj) {
                        for (var i = 0; i < obj.length; i++) {
                            if (obj[i].listeners) {
                                var listeners = obj[i].listeners;
                                for (var l = 0; l < listeners.length; l++) {
                                    chartObj[i].addListener(listeners[l].event, listeners[l].method);
                                }
                            }
                        }
                    };

                    var chartKeys = Object.keys(o);
                    for (var i = 0; i < chartKeys.length; i++) {
                        if (typeof o[chartKeys[i]] !== 'object' && typeof o[chartKeys[i]] !== 'function') {
                            chart[chartKeys[i]] = o[chartKeys[i]];
                        } else if (typeof o[chartKeys[i]] === 'object') {
                            addEventListeners(o[chartKeys[i]], chart[chartKeys[i]]);
                        }
                    }

                    // WRITE
                    chart.write(id);
                    $scope.chart = chart;

                }

                  // EVENTS =========================================================================

                  var onAmChartsTriggerChartAnimate = $scope.$on('amCharts.triggerChartAnimate', function (event, id) {
                    if (id === $el[0].id || !id) {
                      chart.animateAgain();
                    }
                  });

                  var onAmChartsUpdateData = $scope.$on('amCharts.updateData', function (event, data, id) {
                    if (id === $el[0].id || !id) {
                      if($scope.options.type == 'gauge') {
                        if(!Array.isArray(data)) data = [ data ]
                        for(var i = 0; i < data.length; i++) {
                          chart.arrows[i] && chart.arrows[i].setValue && chart.arrows[i].setValue(data[i]);
                        }
                      }
                      else {
                        chart.dataProvider = data;
                        chart.validateData();
                      }
                    }

                  });

                  var onAmChartsValidateNow = $scope.$on('amCharts.validateNow', function (event, validateData, skipEvents, id) {
                    if (id === $el[0].id || !id) {
                      chart.validateNow(validateData === undefined ? true : validateData,
                        skipEvents === undefined ? false : skipEvents);
                    }
                  });

                  var onAmChartsRenderChart = $scope.$on('amCharts.renderChart', function (event, amChartOptions, id) {
                    if (id === $el[0].id || !id) {
                      chart.clear();
                      renderChart(amChartOptions);
                    }
                  });

                  $scope.$on('$destroy', function () {
                    chart.clear();
                    //Unregistering event to prevent slow down;
                    onAmChartsTriggerChartAnimate();
                    onAmChartsUpdateData();
                    onAmChartsValidateNow();
                    onAmChartsRenderChart();
                  });
        }
      });

      function getIdForUseInAmCharts(){
        var id = $scope.id;// try to use existing outer id to create new id

        if (!id){//generate a UUID
          var guid = function guid() {
            function s4() {
              return Math.floor((1 + Math.random()) * 0x10000)
                  .toString(16)
                  .substring(1);
            }

            return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
                s4() + '-' + s4() + s4() + s4();
          };
          id = guid();
        }
        return id;
      }
    }
  };
}]);