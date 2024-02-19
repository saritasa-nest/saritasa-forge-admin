import "/_content/NetForgeBlazor/ckeditor.js";

/**
 * Initializes the CKEditor instance on the given element.
 * @param {HTMLElement} element - The HTML element where the CKEditor should be initialized.
 */
export function InitCKEditor(element) {
    ClassicEditor.create(element);
}
