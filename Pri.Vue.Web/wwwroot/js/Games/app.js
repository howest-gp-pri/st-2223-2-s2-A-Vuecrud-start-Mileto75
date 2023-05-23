let app = new Vue({
    el: "#app",
    data: {
        pageTitle: "Our games",
        products: null,
        product: null,
        url: "https://localhost:7047/api/Games",
        loaded: false,
        product: {
            name: "",
            image: "",
            id: null,
            categories: null,
        },
        addProduct: {
            name: "",
            categories: null,
        }
    },
    created: function () {
        this.getProducts();
    },
    methods: {
        getProducts: async function () {
            //send token
            let headers = {
                headers: {
                    'Authorization': `bearer ${localStorage.getItem("token")}`,
                }
            };
            axios.get(this.url,headers)
                .then(result => this.products = result.data)
                .catch(error => this.error = error.data)
                .finally(() => this.loaded = true);
            console.log(this.products);
        },
        showProductInfo: function(product) {
            console.log(product.name);
            this.product = product;
            $("#productModal").modal('show');
        },
        hideProductInfo: function () {
            console.log("test");
            $("#productModal").modal("hide");
        },
        showAddProductModal: async function () {
            //get the categories
            let categories = await axios.get("https://localhost:7047/api/categories").then(response => response.data.categories)
                .catch(error => console.log(error.response.data))
                .finally(() => { $("#addProductModal").modal('show'); })
            console.log(categories);
            //fill the addProduct categories
            this.addProduct.categories = new Array();
            categories.forEach(category => {
                this.addProduct.categories.push({
                    text: category.name,
                    value: category.id,
                    isSelected: false,
                });
            });
            
        },
        hideAddProductModal: async function () {
            //show the modal
            $("#addProductModal").modal('hide');
        },
        createProduct: async function () {
            //set the formdata
            let formData = new FormData();
            formData.append("Name", this.addProduct.name);
            formData.append("Image", this.$refs.addImage.files[0]);
            this.addProduct.categories.filter(cat => cat.isSelected === true)
                .forEach((cat,index) => {
                    formData.append(`categories[${index}]`,cat.value)
                });
            //set the headers
            let headers = {
                headers: {
                    "Encoding-Type": "multipart/formdata",
                    "Authorization": `bearer ${localStorage.token}`
                }
            }
            //send the request
            await axios.post("https://localhost:7047/api/games", formData, headers)
                .then(response => {
                    console.log(response.status);
                    this.getProducts();
                    this.hideAddProductModal();
                    this.addProduct.name = "";
                    this.$refs.addImage.value = "";
                })
                .catch(error => console.log(error.response.data))
        }
    },
});