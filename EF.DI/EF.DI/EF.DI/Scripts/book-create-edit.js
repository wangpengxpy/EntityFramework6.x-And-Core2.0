(function ($) {
    function Book() {
        var $this = this;
        function initializeAddEditBook() {
            $('.datepicker').datepicker({
                "setDate": new Date(),
                "autoclose": true
            });            
        }
        $this.init = function () {            
            initializeAddEditBook();
        }
    }
    $(function () {
        var self = new Book();
        self.init();
    });
}(jQuery))