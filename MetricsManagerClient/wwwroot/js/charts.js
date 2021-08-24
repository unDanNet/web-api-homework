class ChartsSection {
    #sectionRoot
    #dropdownButton
    #dropdownList
    
    #selectedAgentId
    
    #serverControllerUrl

    #chartStartTime
    #chartEndTime
    
    #chartData
    #chartOptions
    #realTimeChartClearingDelay
    
    #chartRefreshInterval = 5000
    #chartIsPaused = false
    
    
    constructor(sectionParentNode, serverActionUrl, realTimeClearingDelay) {
        this.#sectionRoot = sectionParentNode
        
        this.#dropdownButton = this.#sectionRoot.querySelector('.dropdown')
        this.#dropdownList = this.#sectionRoot.querySelector('.dropdown-list')
        
        this.#serverControllerUrl = serverActionUrl
        
        this.#realTimeChartClearingDelay = realTimeClearingDelay
    }
    
    setupDropdownFunctionality(){
        this.#dropdownButton.addEventListener('click', _ => {
            $.ajax({
                type: 'POST',
                url: '/home/get-all-agents',
                dataType: 'html',
                success: (html) => {
                    this.refreshDropdownOptions(html)
                }
            })
            
            this.#dropdownList.classList.toggle('hidden')
        })
        
        document.addEventListener('click', e => {
            const target = e.target
            
            const listIsShown = !this.#dropdownList.classList.contains('hidden')
            const clickedButton = target == this.#dropdownButton
            const clickedList = target == this.#dropdownList || this.#dropdownList.contains(target)
            
            if (!clickedButton && !clickedList && listIsShown) {
                this.#dropdownList.classList.add('hidden')
            }
        })
    }
    
    refreshDropdownOptions(htmlOptions) {
        this.#dropdownList.innerHTML = ''
        this.#dropdownList.innerHTML = htmlOptions

        this.#dropdownList.querySelectorAll('.dropdown-list__option').forEach(option => {
            option.addEventListener('click', _ => {
                let value = `${option.getAttribute('data-agent-id')}: ${option.getAttribute('data-agent-url')}`

                this.#selectedAgentId = parseInt(option.getAttribute('data-agent-id'))

                this.#dropdownButton.innerText = value
                
                this.#dropdownList.classList.add('hidden')
            })
        })
    }
    
    showError() {
        let chartLocker = this.#sectionRoot.querySelector('.chart-wrapper__chart-locker')
        
        chartLocker.querySelector('div').innerText = "An error occured while getting the metrics. Please, reload the page or choose another agent."
        chartLocker.classList.remove('hidden')
        
        this.#chartIsPaused = true
    }
    
    setupChart(xAxisName, yAxisName, xColumnName, yColumnName) {
        this.#chartData = new google.visualization.DataTable()
        
        this.#chartData.addColumn('date', xColumnName)
        this.#chartData.addColumn('number', yColumnName)
        
        this.#chartOptions = {
            hAxis: {
                title: xAxisName
            },
            vAxis: {
                title: yAxisName
            }
        }
        
        let chart = new google.visualization.LineChart(this.#sectionRoot.querySelector('.chart-wrapper__chart'))
        
        chart.draw(this.#chartData, this.#chartOptions)

        
        let dateParser = (unixMs) => new Date(parseInt(unixMs))
        let dataRowsAmountUntilCleanup = this.#realTimeChartClearingDelay / this.#chartRefreshInterval
        
            
        setInterval(_ => {
            if (this.#selectedAgentId === undefined || this.#chartIsPaused) {
                return
            }
            else {
                this.#sectionRoot.querySelector('.chart-wrapper__chart-locker').classList.add('hidden')
            }
            
            if (this.#chartStartTime === undefined && this.#chartEndTime === undefined) {
                $.ajax({
                    type: 'POST',
                    url: `${this.#serverControllerUrl}/get-last-metric`,
                    data: {
                        agentId: this.#selectedAgentId
                    },
                    dataType: 'json',
                    success: (metric) => {
                        this.#chartData.addRow([
                            dateParser(metric.time),
                            metric.value
                        ])

                        chart.draw(this.#chartData, this.#chartOptions)
                    },
                    error: _ => this.showError()
                })
            } 
            else if (this.#chartStartTime !== undefined && this.#chartEndTime === undefined) {
                $.ajax({
                    type: 'POST',
                    url: `${this.#serverControllerUrl}/get-metrics-from-specified-time`,
                    data: {
                        agentId: this.#selectedAgentId,
                        startTime: this.#chartStartTime
                    },
                    dataType: 'json',
                    success: (metrics) => {
                        for (let i = 0; i < metrics.length; i++) {
                            this.#chartData.addRow([
                                dateParser(metrics[i].time),
                                metrics[i].value
                            ])
                        }

                        chart.draw(this.#chartData, this.#chartOptions)
                        this.#chartStartTime = undefined
                    },
                    error: _ => this.showError()
                })
            }
            else if (this.#chartStartTime === undefined && this.#chartEndTime !== undefined) {
                $.ajax({
                    type: 'POST',
                    url: `${this.#serverControllerUrl}/get-metrics-to-specified-time`,
                    data: {
                        agentId: this.#selectedAgentId,
                        endTime: this.#chartEndTime
                    },
                    dataType: 'json',
                    success: (metrics) => {
                        for (let i = 0; i < metrics.length; i++) {
                            this.#chartData.addRow([
                                dateParser(metrics[i].time),
                                metrics[i].value
                            ])
                        }

                        chart.draw(this.#chartData, this.#chartOptions)
                        this.#chartIsPaused = true
                    },
                    error: _ => this.showError()
                })
            }
            else {
                $.ajax({
                    type: 'POST',
                    url: `${this.#serverControllerUrl}/get-metrics-in-specified-period`,
                    data: {
                        agentId: this.#selectedAgentId,
                        startTime: this.#chartStartTime,
                        endTime: this.#chartEndTime
                    },
                    dataType: 'json',
                    success: (metrics) => {
                        for (let i = 0; i < metrics.length; i++) {
                            this.#chartData.addRow([
                                dateParser(metrics[i].time),
                                metrics[i].value
                            ])
                        }

                        chart.draw(this.#chartData, this.#chartOptions)
                        this.#chartIsPaused = true
                    },
                    error: _ => this.showError()
                })
            }
        }, this.#chartRefreshInterval)
    }
    
    setupChartControl() {
        let chartControlInputStartTime = this.#sectionRoot.querySelector('.chart-control__start-time')
        let chartControlInputEndTime = this.#sectionRoot.querySelector('.chart-control__end-time')
        let chartControlSubmitBtn = this.#sectionRoot.querySelector('.chart-control__submit-btn')
        
        chartControlSubmitBtn.addEventListener('click', _ => {
            let startTimeValue = chartControlInputStartTime.value
            let endTimeValue = chartControlInputEndTime.value
            
            this.#chartStartTime = startTimeValue === '' ? undefined : startTimeValue
            this.#chartEndTime = endTimeValue === '' ? undefined : endTimeValue
            
            this.#chartData.removeRows(0, this.#chartData.getNumberOfRows())
            
            this.#chartIsPaused = false
        })
    }
}
