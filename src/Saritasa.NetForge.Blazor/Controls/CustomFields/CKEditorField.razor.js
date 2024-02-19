import "/_content/NetForgeBlazor/ckeditor.js";

/**
 * Initializes the CKEditor instance on the given element.
 * @param {HTMLElement} element - The HTML element where the CKEditor should be initialized.
 * @param {string} id - A unique ID for the editor.
 * @param {boolean} isReadOnly - A boolean indicating whether the editor should be in read-only mode.
 */
export function InitCKEditor(element, id, isReadOnly) {
    ClassicEditor.create(element)
    .then(editor => {
        if (isReadOnly) {
            editor.enableReadOnlyMode(id);
        }
    })
}
