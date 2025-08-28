document.addEventListener('DOMContentLoaded', function () {
    const opblockObserver = new MutationObserver((mutationsList) => {
        for (const mutation of mutationsList) {
            if (mutation.type === 'childList') {
                const downloadAnchor = mutation.target.querySelector('a[download]')
                if (downloadAnchor) {
                    downloadAnchor.target = '_blank'
                    downloadAnchor.removeAttribute('download')
                }
            }
        }
    });
    let swaggerUiObserver = new MutationObserver((mutationsList) => {
        for (const mutation of mutationsList) {
            if (mutation.type === 'childList') {
                const opblocks = document.querySelectorAll('.opblock')
                
                if(opblocks.length > 0) {
                    opblocks.forEach(opblock => {
                        opblockObserver.observe(opblock, {
                            childList: true,
                            subtree: true
                        });
                    });

                    swaggerUiObserver.disconnect();
                    swaggerUiObserver = null
                }
            }
        }
    });
    
    swaggerUiObserver.observe(document.querySelector('#swagger-ui'), {
        childList: true,
        subtree: true
    })
});