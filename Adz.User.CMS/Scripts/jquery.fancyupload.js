var initSelect = function (context) {
    context = context || $(document);
    //Get the button of the form of the context
    var button = $('button', context).attr('disabled', true);
    //Get the checkboxes and disable them
    var boxes = $('input[type=checkbox]', context).attr('disabled', true);
    //Get the form of the context
    var form = $('form', context);

    //Get the preview image and set the onload event handler
    var preview = $('#preview', context).load(function () {
        setPreview();
        /*ias.setOptions({
            x1: 0,
            y1: 0,
            x2: $(this).width(),
            y2: $(this).height(),
            show: true
        });*/
    });

    //Set the 4 coordinates for the cropping
    var setPreview = function (x, y, w, h) {
        $('#X', context).val(x || 0);
        $('#Y', context).val(y || 0);
        $('#Width', context).val(w || preview[0].naturalWidth);
        $('#Height', context).val(h || preview[0].naturalHeight);
    };

    //Initialize the image area select plugin
    var ias = preview.imgAreaSelect({
        handles: true,
        instance: true,
        parent: 'body',
        onSelectEnd: function (s, e) {
            var scale = preview[0].naturalWidth / preview.width();
            var _f = Math.floor;
            setPreview(_f(scale * e.x1), _f(scale * e.y1), _f(scale * e.width), _f(scale * e.height));
        }
    });

    //Check one of the checkboxes
    var setBox = function (filter) {
        button.attr('disabled', false);
        boxes.attr('checked', false)
            .filter(filter).attr({ 'checked': true, 'disabled': false });
    };

    //Fallback for Browsers with no FileAPI
    var filePreview = function () {
        window.callback = function () { };
        $('body').append('<iframe id="preview-iframe" onload="callback();" name="preview-iframe" style="display:none"></iframe>');
        var action = $('form', context).attr('target', 'preview-iframe').attr('action');
        form.attr('action', '/Image/PreviewImage');
        window.callback = function () {
            setBox('#IsFile');
            var result = $('#preview-iframe').contents().find('img').attr('src');
            preview.attr('src', result);
            $('#preview-iframe').remove();
        };
        form.submit().attr('action', action).attr('target', '');
    };

    //Initial state of X, Y, Width and Height is 0 0 1 1
    setPreview(0, 0, 1, 1);

    //Fetch Flickr images
    var fetchImages = function (query) {
        $.getJSON('http://www.flickr.com/services/feeds/photos_public.gne?jsoncallback=?', {
            tags: query,
            tagmode: "any",
            format: "json"
        }, function (data) {
            var screen = $('<div />').addClass('waitScreen').click(function () {
                $(this).remove();
            }).appendTo('body');
            var box = $('<div />').addClass('flickrImages').appendTo(screen);
            $.each(data.items, function (i, v) {
                console.log(data.items[i]);
                $('<img />').addClass('flickrImage').attr('src', data.items[i].media.m).click(function () {
                    $('#Flickr', context).val(this.src).change();
                    screen.remove();
                }).appendTo(box);
            });
        });
    };

    //Flickr
    $('#FlickrQuery', context).change(function () {
        fetchImages(this.value);
    });

    $('#Flickr', context).change(function () {
        setBox('#IsFlickr');
        preview.attr('src', this.value);
    });

    //What happens if the URL changes?
    $('#Url', context).change(function () {
        setBox('#IsUrl');
        preview.attr('src', this.value);
    });

    //What happens if the File changes?
    $('#File', context).change(function (evt) {
        if (evt.target.files === undefined)
            return filePreview();

        var f = evt.target.files[0];
        var reader = new FileReader();

        if (!f.type.match('image.*')) {
            alert("The selected file does not appear to be an image.");
            return;
        }

        setBox('#IsFile');
        reader.onload = function (e) { preview.attr('src', e.target.result); };
        reader.readAsDataURL(f);
    });

    //What happens if any checkbox is checked ?!
    boxes.change(function () {
        setBox(this);
        $('#' + this.id.substr(2), context).change();
    });

    form.submit(function () {
        button.attr('disabled', true).text('Please wait ...');
    });
};