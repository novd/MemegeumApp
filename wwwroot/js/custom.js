updateTagLists();

function updateTagLists() {
    updateWhiteListTags();
    updateBlackListTags();
}

function updateWhiteListTags() {
    var whiteListInput = document.getElementById("whiteListTagsInput");
    var whiteListInputValue = whiteListInput.getAttribute("value");

    var whiteListTagsArray=[""];
    if (!(whiteListInputValue == null)) {
        whiteListTagsArray = whiteListInputValue.split("#");
    }

    whiteListTagsArray.forEach(function (item) {
        if (item) {
            $("#whiteListTags").append(createTagElement(item));
        }
    });
}

function updateBlackListTags() {
    var blackListInput = document.getElementById("blackListTagsInput");
    var blackListInputValue =blackListInput.getAttribute("value");

    var blackListTagsArray =[""];
    if (!(blackListInputValue == null)) {
        blackListTagsArray =blackListInputValue.split("#");
    }

    blackListTagsArray.forEach(function(item){
        if (item) {
            $("#blackListTags").append(createTagElement(item));
        }
    });
}


function createTagElement(tagText) {
    var liTagElement = document.createElement("li");
    $(liTagElement).addClass("tag-element");

    var paragraph = $("<p></p>").text("#" + tagText);

    var divButtonContainer = document.createElement("div");
    $(divButtonContainer).addClass("tag-options-container");

    var removeButton = document.createElement("button");
    $(removeButton).addClass("custom-btn btn-black-list");
    $(removeButton).text("USUŃ");
    $(removeButton).click(function() {moveParentToDefaultList(removeButton)});

    $(liTagElement).append(paragraph);
    $(liTagElement).append(divButtonContainer);
    $(divButtonContainer).append(removeButton);

    return liTagElement;
}


function moveParentToWhiteList(element) {
    var divContainingButtons = element.parentElement;
    appendTagToWhiteListTagInput(divContainingButtons.parentElement);

    $(divContainingButtons).children().remove();

    var removeButton = document.createElement("button");
    removeButton.classList.add("custom-btn");
    removeButton.classList.add("btn-black-list");
    removeButton.innerText="USUŃ";
    removeButton.onclick=function() {moveParentToDefaultList(removeButton)};

    divContainingButtons.appendChild(removeButton);

    document.getElementById("whiteListTags").appendChild(divContainingButtons.parentElement);
    }

function moveParentToBlackList(element) {
    var divContainingButtons = element.parentElement;
    appendTagToBlackListTagInput(divContainingButtons.parentElement);

    $(divContainingButtons).children().remove();

    var removeButton = document.createElement("button");
    removeButton.classList.add("custom-btn");
    removeButton.classList.add("btn-black-list");
    removeButton.innerText="USUŃ";
    removeButton.onclick=function() {moveParentToDefaultList(removeButton)};

    divContainingButtons.appendChild(removeButton);

    document.getElementById("blackListTags").appendChild(divContainingButtons.parentElement);
}

function moveParentToDefaultList(element) {
    var liTag=element.parentElement.parentElement;

    removeTagFromBlackListTagInput(liTag);
    removeTagFromWhiteListTagInput(liTag);

    document.getElementById("allTags").appendChild(liTag);
}

function searchInUl(searchInput, ulId) {
  var filter, ul, li, p, i, txtValue;
  filter = searchInput.value.toUpperCase();
  ul = document.getElementById(ulId);
  li = ul.getElementsByTagName('li');

  for (i = 0; i < li.length; i++) {
    p = li[i].getElementsByTagName("p")[0];
    txtValue = p.textContent || p.innerText;
    if (txtValue.toUpperCase().indexOf(filter) > -1) {
      li[i].style.display = "";
    } else {
      li[i].style.display = "none";
    }
  }
}

function appendTagToWhiteListTagInput(elementContainingTag) {
    var tagText = elementContainingTag.getElementsByTagName("p")[0].innerText;
    var tagInput = document.getElementById("whiteListTagsInput");

    var oldInputValue=tagInput.getAttribute("value");
    var updatedInputValue = oldInputValue + tagText;

    tagInput.setAttribute("value", updatedInputValue);
}

function appendTagToBlackListTagInput(elementContainingTag) {
    var tagText = elementContainingTag.getElementsByTagName("p")[0].innerText;
    var tagInput = document.getElementById("blackListTagsInput");

    var oldInputValue=tagInput.getAttribute("value");
    var updatedInputValue = oldInputValue + tagText;

    tagInput.setAttribute("value", updatedInputValue);
}

function removeTagFromWhiteListTagInput(elementContainingTag) {
    var tagText = elementContainingTag.getElementsByTagName("p")[0].innerText;
    var tagInput = document.getElementById("whiteListTagsInput");

    var oldInputValue=tagInput.getAttribute("value");
    var updatedInputValue = oldInputValue.replace(tagText,"");

    tagInput.setAttribute("value", updatedInputValue);
}

function removeTagFromBlackListTagInput(elementContainingTag) {
    var tagText = elementContainingTag.getElementsByTagName("p")[0].innerText;
    var tagInput = document.getElementById("blackListTagsInput");

    var oldInputValue=tagInput.getAttribute("value");
    var updatedInputValue = oldInputValue.replace(tagText,"");

    tagInput.setAttribute("value", updatedInputValue);
}
