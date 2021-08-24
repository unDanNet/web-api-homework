function addClickListenersInAgentsTable() {
    let labels = document.querySelectorAll('.agents-table__label')
    
    labels.forEach(label => {
        label.addEventListener('click', e => {
            let agentId = label.previousElementSibling.getAttribute('data-agentId')
            
            let tableRow = label.parentNode.parentNode
            let checkbox = label.previousElementSibling
            
            if (!checkbox.checked) {
                $.ajax({
                    type: 'PUT',
                    url: '/home/enable-agent',
                    data: {
                        id: parseInt(agentId)
                    },
                    success: _ => {
                        tableRow.classList.remove('agent-disabled')
                        tableRow.classList.add('agent-enabled')
                        
                        label.children[0].textContent = 'Enabled'
                    },
                    error: (jqxhr, textStatus, errorThrown) => {
                        alert(`${textStatus} - ${errorThrown}`)
                    }
                })
            } else {
                $.ajax({
                    type: 'PUT',
                    url: '/home/disable-agent',
                    data: {
                        id: parseInt(agentId)
                    },
                    success: _ => {
                        tableRow.classList.remove('agent-enabled')
                        tableRow.classList.add('agent-disabled')
                        label.children[0].textContent = 'Disabled'
                    },
                    error: (jqxhr, textStatus, errorThrown) => {
                        alert(`${textStatus} - ${errorThrown}`)
                    }
                })
            }
        })
    })
}

function showInputError(inputField) {
    let errorBlock = document.querySelector(".agents-table__form .inline-form__error")

    inputField.style.cssText = 'border: 1px solid #7d0011'
    errorBlock.classList.remove('hidden')
}

function addSubmitHandlerForAgentsTableForm() {
    let input = document.querySelector(".agents-table__form .inline-form__input-field")
    let submitBtn = document.querySelector(".agents-table__form .inline-form__submit-btn")
    
    submitBtn.addEventListener('click', () => {
        let inputValue = input.value
        
        if (inputValue === '') {
            showInputError(input)
        } else {
            $.ajax({
                type: 'POST',
                url: '/home/add-agent',
                data: {
                    url: inputValue
                },
                dataType: 'html',
                success: (html) => {
                    $(".agents-table .table-body").append(html)
                    addClickListenersInAgentsTable()
                },
                error: _ => {
                    showInputError(input)
                }
            })
        }
    })
}

$(document).ready(function(){
    addClickListenersInAgentsTable()

    addSubmitHandlerForAgentsTableForm()

    
    let chartsSections = [
        new ChartsSection(document.querySelector('.cpu-dashboard'), '/cpu', 180000),
        new ChartsSection(document.querySelector('.ram-dashboard'), '/ram', 180000),
        new ChartsSection(document.querySelector('.network-dashboard'), '/network', 180000),
        new ChartsSection(document.querySelector('.hdd-dashboard'), '/hdd', 180000),
        new ChartsSection(document.querySelector('.dotnet-dashboard'), '/dotnet', 180000)
    ]

    for (let i = 0; i < chartsSections.length; i++) {
        chartsSections[i].setupDropdownFunctionality()
        chartsSections[i].setupChartControl()
    }

    google.charts.load('current', {packages: ['corechart', 'line']})
    google.charts.setOnLoadCallback(function () {
        chartsSections[0].setupChart('Time', 'Cpu usage', 'X', '% of processor time')
        chartsSections[1].setupChart('Time', 'Ram usage', 'X', 'MBytes available')
        chartsSections[2].setupChart('Time', 'Network usage', 'X', 'Packets/sec')
        chartsSections[3].setupChart('Time', 'Disk usage', 'X', 'Space left')
        chartsSections[4].setupChart('Time', '.NET errors', 'X', 'Errors count')
    })
})
