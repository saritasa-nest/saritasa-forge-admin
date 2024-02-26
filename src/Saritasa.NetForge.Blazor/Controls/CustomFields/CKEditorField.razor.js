import "/_content/NetForgeBlazor/ckeditor.js";

const editors = [];
const timeouts = [];

/**
 * Initializes the CKEditor instance on the given element.
 * @param {HTMLElement} element - The HTML element where the CKEditor should be initialized.
 * @param {string} id - A unique ID for the editor.
 * @param {boolean} isReadOnly - A boolean indicating whether the editor should be in read-only mode.
 */
export function InitCKEditor(element, id, isReadOnly, dotnetReference) {
    ClassicEditor.defaultConfig = {
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

    ClassicEditor.create(element)
    .then(editor => {
        editors[id] = editor;
        if (isReadOnly) {
            editor.enableReadOnlyMode(id);
        }
        else {
            editor.model.document.on('change:data', () => {
                if (timeouts[id]) {
                    clearTimeout(timeouts[id]);
                    delete timeouts[id];
                }

                // Invoke UpdateText method after a delay.
                timeouts[id] = setTimeout(() => {
                    dotnetReference.invokeMethodAsync('UpdateText', editor.getData());
                }, 50);
            });
        }
    })
}
