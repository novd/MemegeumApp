var coll = document.getElementsByClassName("collapsible");
var i;

for (i = 0; i < coll.length; i++) {
  coll[i].addEventListener("click", function() {
    this.classList.toggle("active");
    var content = this.nextElementSibling;
    if (content.style.maxHeight){
      content.style.maxHeight = null;
    } else {
        if(content.classList.contains("tag-container")){
            content.style.maxHeight = 150 + "px";
        }
        else {
            content.style.maxHeight=750 +"px";
        }
    } 
  });
}
