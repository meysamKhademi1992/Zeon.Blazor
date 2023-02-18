
function ZeonFocusElementById(id) {
    try {
        const element = document.getElementById(id);
        element.focus();
    } catch (e) {
        console.log(e);
    }
};

function ZeonScrollToElementById(id, isTop) {

    const element = document.getElementById(id);
    element.scrollIntoView(isTop);
}