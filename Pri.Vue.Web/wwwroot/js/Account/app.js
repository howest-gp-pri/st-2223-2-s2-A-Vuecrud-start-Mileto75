let app = new Vue({
    el: "#app",
    data: {
        registerPageTitle: "Register",
        loginPageTitle: "Login",
        registerDto: {
            username: "",
            firstname: "",
            lastname: "",
            password: "",
            checkPassword: "",
            dateOfBirth: null,
        },
        loginDto: {
            username: "",
            password: "",
        },
        hasMessage: false,
        message: "",
        hasError: false,
        loading: false,
        errors: null,
        registerUrl: "https://localhost:7047/api/account/register",
        loginUrl: "https://localhost:7047/api/account/login",
    },
    methods: {
        register: async function () {
            this.hasError = false;
            this.hasMessage = false;
            //call the axios post method
            //set loading
            this.loading = true;
            //with the postData
            await axios.post(this.registerUrl, this.registerDto)
                .then(() => {
                    //handle the response
                   this.message = "User registered!";
                   this.hasMessage = true;
                })
                .catch(error => {
                    this.handleError(error);
                })
                .finally(() => this.loading = false);
        },
        login: async function () {
            this.hasError = false;
            this.loading = true;
            let token = await axios.post(this.loginUrl, this.loginDto)
                .then((response) => response.data)
                .catch(error => {
                    this.handleError(error);
                })
                .finally(() => {
                    this.loading = false;
                });
            if (this.hasError == false) {
                this.hasMessage = true;
                this.message = "Logged in!";
            };
            localStorage.setItem("token", token);
        },
        handleError: function (error) {
            console.log(error.response.data);
            this.errors = new Array();
            this.hasError = true;
            if (typeof (error.response.data) === "string")
                this.errors.push([error.response.data]);
            else
                this.errors = error.response.data.errors;
        }
    },
});