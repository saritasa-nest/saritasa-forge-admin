import "/_content/NetForgeBlazor/ckeditor.js";

const timeouts = [];
const editorConfig = {
    toolbar: {
        items: [
            'heading',
            '|',
            'bold',
            'italic',
            '|',
            'bulletedList',
            'numberedList',
            '|',
            'insertTable',
            '|',
            '|',
            'mediaEmbed',
            'undo',
            'redo'
        ]
    },
    image: {
        toolbar: [
            'imageStyle:full',
            'imageStyle:side',
            '|',
            'imageTextAlternative'
        ]
    },
    table: {
        contentToolbar: ['tableColumn', 'tableRow', 'mergeTableCells']
    },
    language: 'en'
};

/**
 * Initializes the CKEditor instance on the given element.
 * @param {HTMLElement} element - The HTML element where the CKEditor should be initialized.
 * @param {string} id - A unique ID for the editor.
 * @param {boolean} isReadOnly - A boolean indicating whether the editor should be in read-only mode.
 * @param dotnetReference {Object} - DotNet object reference.
 */
export function InitCKEditor(element, id, isReadOnly, dotnetReference) {
    ClassicEditor.defaultConfig = editorConfig;

    ClassicEditor.create(element)
    .then(editor => {
        if (isReadOnly) {
            editor.enableReadOnlyMode(id);
        }
        else {
            editor.model.document.on('change:data', () => {
                if (timeouts[id]) {
                    clearTimeout(timeouts[id]);
                    delete timeouts[id];
                }

                // Update the text after a delay.
                timeouts[id] = setTimeout(() => {
                    dotnetReference.invokeMethodAsync('UpdateTextAsync', editor.getData());
                }, 50);
            });
        }
    })
}
