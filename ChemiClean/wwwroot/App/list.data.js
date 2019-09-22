(function () {
    'use strict';

    angular
        .module('app.products')
        .factory('productsService', productsService);

    productsService.$inject = ['$http'];

    function productsService($http) {
        var productsService =
        {
            getProducts: getProducts,
            getSuppliers: getSuppliers,
            checkAvailability: checkAvailability
        };

        return productsService;
        
        function getProducts(keyWord, pageNumber, pageSize, supplierName) {
            return $http.get('/api/Products/GetProducts', {
                params: {
                    keyWord: keyWord,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    supplierName: supplierName
                }
            });
        }
        function getSuppliers() {
            return $http.get('/api/Products/GetSuppliers');
        }

        function checkAvailability(productId) {
            return $http.get('/api/Products/CheckProductDataSheetAvailability', {
                params: {
                    productId: productId
                }
            });
        }


        

    }
}());