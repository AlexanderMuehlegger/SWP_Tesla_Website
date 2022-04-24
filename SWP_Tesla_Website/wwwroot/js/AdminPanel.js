﻿const selected_obj = document.querySelector(".selected");
const option_container = document.querySelector(".option-container");

const cards = document.querySelectorAll(".card");

const car_switch = document.querySelector(".car-mode-switch");
const user_switch = document.querySelector(".user-mode-switch");

const car_container = document.querySelector(".car-container")
const user_container = document.querySelector(".user-container")

const filter_car = ['Standard Range', 'Long Range', 'Performance', 'Plaid', 'No Filter']
const filter_user = ['Admin', 'Dev', 'User', 'Banned', 'No Filter']


function unloadFilter(){
    if(option_container.hasChildNodes())
        while(option_container.firstChild)
            option_container.removeChild(option_container.lastChild)
    else
        console.log("Currently no filters loaded!")
}

function loadCarFilter(){
    if(option_container == null)
        return;
    
    unloadFilter()

    filter_car.forEach(filter => {
        var div = document.createElement('div')
        div.classList.add("option")
    
        var input = document.createElement('input')
        input.classList.add("radio")
        input.type = 'radio';
        input.name = 'type';
        input.id = filter == 'No Filter'? filter.replace(' ', '_') : filter.includes(' ')? filter.split(' ')[0]  : filter;
    
        var label = document.createElement('label')
        label.for = filter == 'No Filter'? filter.replace(' ', '_') : filter.includes(' ')? filter.split(' ')[0]  : filter;
        label.innerText = filter;
    
        div.appendChild(input)
        div.appendChild(label)
        option_container.appendChild(div)
    })
    initOptions()
}

function loadUserFilter(){
    if(option_container == null)
        return;
    
    unloadFilter()
    
    filter_user.forEach(filter => {
        var div = document.createElement('div')
        div.classList.add('option')

        var input = document.createElement('input')
        input.classList.add('radio')
        input.type = 'radio'
        input.name = 'type'
        input.id = filter == 'No Filter'? filter.replace(' ', '_') : filter.includes(' ')? filter.split(' ')[0] : filter

        var label = document.createElement('label')
        label.for = filter == 'No Filter'? filter.replace(' ', '_') : filter.includes(' ')? filter.split(' ')[0]  : filter;
        label.innerText = filter;

        div.appendChild(input)
        div.appendChild(label)
        option_container.appendChild(div)
    })
    initOptions()
}

selected_obj.addEventListener('click', () => {
    if(option_container == null)
        return;

    option_container.classList.toggle("active")
});

function initOptions(){
    document.querySelectorAll(".option").forEach(i => {
        i.addEventListener('click', () => {
            var innerText = i.querySelector("label").innerHTML;
            selected_obj.innerHTML = innerText
            option_container.classList.remove("active")
    
            cards.forEach(card => {
                if(innerText == "No Filter"){
                    card.style.display = ''
                }else {
                    if(!card.querySelector(".model.second").innerHTML.includes(innerText))
                        card.style.display = 'none';
                    else
                        card.style.display = ''
                }
            })
        })
    })
}

car_switch.addEventListener('click', () => {
    setCarState()
})

user_switch.addEventListener('click', () => {
    setUserState()
})

function setCarState(){
    user_container.style.display = 'none'
    car_container.style.display = ''
    loadCarFilter()
}

function setUserState(){
    user_container.style.display = ''
    car_container.style.display = 'none'
    loadUserFilter()
}

