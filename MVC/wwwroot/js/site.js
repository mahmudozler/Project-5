// Write your JavaScript code.
function search(value, e) {
    if (e.keyCode === 13) {
        e.preventDefault();
        window.location.replace("http://localhost:5000/product?searchString=" + value);
    }
}