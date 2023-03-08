
function ZeonFocusElementById(id) {
    try {
        const element = document.getElementById(id);
        element.focus();
    } catch (e) {
        console.log(e);
    }
};

function ZeonScrollToElementById(elementId, itemId) {

    const element = document.getElementById(elementId);
    const item = document.getElementById(itemId);
    element.scrollTo({ top: item.offsetTop, behavior: 'smooth' });
}