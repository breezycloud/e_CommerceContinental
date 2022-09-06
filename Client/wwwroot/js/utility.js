function printDialog() {
    var printButton = document.getElementById("hide-print");
    printButton.style.visibility = 'hidden';
    window.print();
    printButton.style.visibility = 'hidden';

}