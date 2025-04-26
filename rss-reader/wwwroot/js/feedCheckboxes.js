document.addEventListener("DOMContentLoaded", function () {
    const feedCheckboxes = document.querySelectorAll(".feed-checkbox");
    const selectAll = document.getElementById("selectAll");
    const deleteDiv = document.getElementById("deleteDiv");
    
    function updateDeleteDivVisibility() {
        const anyChecked = Array.from(feedCheckboxes).some(cb => cb.checked);
        deleteDiv.classList.toggle("invisible", !anyChecked);
    }

    if (selectAll) {
        selectAll.addEventListener("change", function() {
            feedCheckboxes.forEach(cb => {
                cb.checked = selectAll.checked;
            });
            updateDeleteDivVisibility();
        });
    }

    feedCheckboxes.forEach(cb => {
        cb.addEventListener("change", function() {
            selectAll.checked = Array.from(feedCheckboxes).every(c => c.checked);
            updateDeleteDivVisibility();
        });
    });

    window.deleteSelectedFeeds = function() {
        const selectedCheckboxes = document.querySelectorAll(".feed-checkbox:checked");
        const selectedFeedIds = Array.from(selectedCheckboxes).map(cb => cb.value);
        
        openModal(selectedFeedIds);
    };
});
