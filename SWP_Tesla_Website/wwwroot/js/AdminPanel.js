const selected_obj = document.querySelector(".selected");
const option_container = document.querySelector(".option-container");

const option_list = document.querySelectorAll(".option")

selected_obj.addEventListener('click', () => {
    option_container.classList.toggle("active")
});

option_list.forEach(i => {
    i.addEventListener('click', () => {
        selected_obj.innerHTML = i.querySelector("label").innerHTML
        option_container.classList.remove("active")
    })
    i.addEventListener('mouseenter', () => {
        if(selected_obj.innerHTML === i.querySelector("label").innerHTML){
            i.style.cursor = "not-allowed"
            i.querySelector("label").style.cursor = "not-allowed"
        }
    })
})