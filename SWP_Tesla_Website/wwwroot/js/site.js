function toggleSideBar() {
    const sideBar = document.querySelector(".primary-sidebar");
    const isVisible = sideBar.getAttribute('isVisible');

    if (isVisible === "true") {
        sideBar.setAttribute('isVisible', false);
    } else {
        sideBar.setAttribute('isVisible', true);
    }
}

function setAttributeOfWebsite(visibility) {
    const allElementsWithAttribute = getAllElementsWithAttribute("is-in-login");

    if (allElementsWithAttribute.length === 0)
        return -1;

    for (var i = 0; i < allElementsWithAttribute.length; i++) {
        allElementsWithAttribute[i].setAttribute("is-in-login", visibility);
    }
}

function getAllElementsWithAttribute(attribute) {
    const allElements = document.getElementsByTagName('*');
    const matchingElements = [];

    for (var i = 0; i < allElements.length; i++) {
        if (allElements[i].getAttribute(attribute) !== null) {
            matchingElements.push(allElements[i]);
        }
    }

    return matchingElements;
}

function changeCompleteSiteState(setToState) {
    const circle = document.querySelector(".background-circle");

    const elementsForChange = getAllElementsWithAttribute("currentSiteState");

    for (var i = 0; i < elementsForChange.length; i++) {
        elementsForChange[i].setAttribute("currentSiteState", setToState);
    }

    circle.setAttribute("stateOfCircle", setToState);
    
}

function changeToRegister() {
    changeCompleteSiteState("register");
}

function changeToLogin() {
    changeCompleteSiteState("login");
}