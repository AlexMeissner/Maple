function initializeSortable(elementId, handleId, styleId, component) {
    const tableBody = document.getElementById(elementId);

    if (!tableBody) {
        console.error('Could not initialize sortable. ' + elementId + ' not found');
        return;
    }

    Sortable.create(tableBody, {
        animation: 150,
        handle: handleId,
        ghostClass: styleId,
        onUpdate: (event) => {
            event.item.remove();
            event.to.insertBefore(event.item, event.to.childNodes[event.oldIndex]);

            component.invokeMethodAsync('OnMoveItem', event.oldDraggableIndex, event.newDraggableIndex);
        }
    });
}
