$(document).ready(function () {
    // EDIT PAGE - single image preview
    $('#editprofileImg').on('change', function () {
       
        const file = this.files[0];

        if (file && file.type.startsWith("image/")) {
            // Remove the <span> message if it exists
            $('#text').remove();

            // Check if preview image already exists
            let $previewImg = $('#preImg');
            if ($previewImg.length === 0) {
                $previewImg = $('<img>', {
                    id: 'preImg',
                    class: 'img-thumbnail',
                    css: { 'max-width': '150px' }
                });

                $('.row.mx-2').append($previewImg);
            }

            const reader = new FileReader();
            reader.onload = function (e) {
                $previewImg.attr('src', e.target.result);
            };
            reader.readAsDataURL(file);
        }
    });


    // CREATE PAGE - multiple image preview
    $('#profileImg').on('change', function (e) {
        const files = e.target.files;
        const preview = $('#preview');
        const uploadLabel = $('#uploadLabel');
        const previewText = $('#previewText');

        preview.empty(); // Clear previous images

        if (files.length > 0) {
            uploadLabel.text('Image uploaded successfully').removeClass('text-danger').addClass('text-success');
            previewText.text('Selected Image Preview');
        }

        $.each(files, function (i, file) {
            if (file.type.startsWith("image/")) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    const img = $('<img>', {
                        src: e.target.result,
                        alt: file.name,
                        css: {
                            width: '150px',
                            margin: '5px',
                            padding: '10px',
                            border: '2px solid black'
                        }
                    });
                    preview.append(img);
                };
                reader.readAsDataURL(file);
            } else {
                alert(`${file.name} is not an image file.`);
            }
        });
    });

});
