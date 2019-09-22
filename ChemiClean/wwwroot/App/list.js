(function () {
    'use strict';

    angular
        .module('app.products')
        .controller('ProductsListingController', ProductsListingController);

    ProductsListingController.$inject = ['productsService'];

    function ProductsListingController(productsService) {
        var productsListingCntrl = this;
        productsListingCntrl.suppliers = [];
        productsListingCntrl.products = [];
        productsListingCntrl.searchProducts = searchProducts;
        productsListingCntrl.LoadMore = LoadMore;
        productsListingCntrl.searchCriteria = {};
        productsListingCntrl.getDownloadLink = getDownloadLink;
        productsListingCntrl.checkAvailability = checkAvailability;
        // Page size should be configurations
        productsListingCntrl.searchCriteria.pageSize = 9;
        productsListingCntrl.searchCriteria.pageNumber = 1;
        productsListingCntrl.loading = true;
        init();

        function init() {
            productsService.getSuppliers().then(getSuppliersSucceded, getSuppliersFailed);
            productsService.getProducts('', 1, productsListingCntrl.searchCriteria.pageSize).then(getProductsSucceded, getProductsFailed);
        }
        function getProductsSucceded(result) {
            if (result && result.data) {
                console.log(result);
                productsListingCntrl.products = result.data.products;
                productsListingCntrl.totalCount = result.data.totalCount;
                productsListingCntrl.loading = false;
            }
        }
        function getProductsFailed(err) {
            console.log(err);
        }
        function searchProducts() {
            productsListingCntrl.loading = true;
            productsListingCntrl.searchCriteria.pageNumber = 1;
            productsService.getProducts(productsListingCntrl.searchCriteria.keyword, productsListingCntrl.searchCriteria.pageNumber, productsListingCntrl.searchCriteria.pageSize, productsListingCntrl.selectedsupplier).then(getProductsSucceded, getProductsFailed);
        }
        function getSuppliersSucceded(result) {
            if (result && result.data) {
                console.log(result);
                productsListingCntrl.suppliers = result.data;
            }
        }
        function getSuppliersFailed(err) {
            console.log(err);
        }
        function getDownloadLink(product) {
            return '/DownloadFile/DownloadFileURL/' + product.id;
        }
        function LoadMore() {
            productsListingCntrl.loading = true;
            productsListingCntrl.searchCriteria.pageNumber++;
            productsService.getProducts(productsListingCntrl.searchCriteria.keyword, productsListingCntrl.searchCriteria.pageNumber, productsListingCntrl.searchCriteria.pageSize, productsListingCntrl.selectedsupplier).then(LoadMoreProductsSucceded, getProductsFailed);
        }
        function LoadMoreProductsSucceded(result) {
            if (result && result.data) {
                console.log(result);
                productsListingCntrl.products = productsListingCntrl.products.concat(result.data.products);
                productsListingCntrl.totalCount = result.data.totalCount;
                productsListingCntrl.loading = false;
            }
        }
        function checkAvailability(product) {
            productsListingCntrl.checkingDocumentAvaliable = product.id;
            productsListingCntrl.currentProduct = product;
            productsService.checkAvailability(product.id).then(checkAvailabilitySucceded, checkAvailabilityFailed);
        }
        function checkAvailabilitySucceded(result) {
            if (result) {
                productsListingCntrl.currentProduct.fileAvaliable = result.data;
            }
            productsListingCntrl.checkingDocumentAvaliable = 0;
        }
        function checkAvailabilityFailed(err) {
            console.log(err);
        }
        productsListingCntrl.checkingDocumentAvaliable = 0;
    }
})();