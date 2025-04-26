window.openModal = function (feedIdOrList, feedName) {
    const deleteFeedId = document.getElementById("deleteFeedId");
    const deleteModalBody = document.getElementById("deleteModalBody");
    const modal = document.getElementById("deleteModal");

    if (Array.isArray(feedIdOrList)) {
        const count = feedIdOrList.length;
        deleteFeedId.value = feedIdOrList.join(",");
        deleteModalBody.textContent = `Do you really want to delete ${count} feed${count>1?'s':''}?`;
    } else {
        deleteFeedId.value = feedIdOrList;
        deleteModalBody.textContent = `Do you really want to delete feed: ${feedName}?`;
    }

    modal.classList.replace("hidden", "flex");
};

window.closeModal = function () {
    const modal = document.getElementById("deleteModal");
    if (modal) {
        modal.classList.replace("flex", "hidden");
    }
};
