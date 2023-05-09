
function zeonFocusElementById(id) {
    try {
        const element = document.getElementById(id);
        element.focus();
    } catch (e) {
        console.log(e);
    }
};

function zeonScrollToElementById(elementId, itemId) {

    const element = document.getElementById(elementId);
    const item = document.getElementById(itemId);
    element.scrollTo({ top: item.offsetTop, behavior: 'smooth' });
}

function zeonScrollIntoViewById(elementId) {

    const element = document.getElementById(elementId);
    element.scrollIntoView({ behavior: 'smooth' });
}

function zeonAddClassById(elementId, className) {

    const element = document.getElementById(elementId);
    if (hasClass(element, className))
        element.classList.add(className);
}

function zeonRemoveClassById(elementId, className) {

    const element = document.getElementById(elementId);
    if (hasClass(element, className))
        element.classList.remove(className);

}

function zeonToggleClassById(elementId, className) {

    const element = document.getElementById(elementId);
    element.classList.toggle(className);

}

function hasClass(element, className) {
    return (' ' + element.className + ' ').indexOf(' ' + className + ' ') > -1;
}
