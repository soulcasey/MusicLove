ShowImageSelect()

function ShowImageSelect() {
    var value = $("#AnimalSelect").val()

    if (value == 0){
        $('#ImageSelect').hide();
    }
    else{
        $('#ImageSelect').show();
    }
}

function Delete(url) {

    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    location.reload();
                }
            })
        }
    })
}