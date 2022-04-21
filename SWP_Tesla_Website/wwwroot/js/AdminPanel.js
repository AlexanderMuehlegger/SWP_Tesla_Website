const selected_obj = document.querySelector(".selected");
const option_container = document.querySelector(".option-container");

const option_list = document.querySelectorAll(".option")
const cards = document.querySelectorAll(".card");

selected_obj.addEventListener('click', () => {
    option_container.classList.toggle("active")
});

option_list.forEach(i => {
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

