<template>
    <form @submit="submitEntry">
        <input type="text" v-model="firstName" name="firstName" placeholder="First Name..." />
        <input type="text" v-model="lastName" name="lastName" placeholder="Last Name..." />
        <input type="text" v-model="email" name="email" placeholder="Email..." />
        <input type="text" v-model="serialNumber" name="serialNumber" placeholder="Serial Number..." />
        <input type="submit" value="submit" />
    </form>
</template>

<script>

export default {
    name: "PrizeDrawForm",
    props: ['validationRules'],
    data() {

        return {
            firstName: '',
            lastName: '',
            email: '',
            serialNumber: ''
        }
    },
    methods: {

        submitEntry(e) {

            e.preventDefault();

            // validate the input
            const validates = this.validateForm();

            if(validates === true)
            {
                const newPDrawSub = {

                    firstName: this.firstName,
                    lastName: this.lastName,
                    email: this.email,
                    serialNumber: this.serialNumber
                };

                this.$emit('successfulSubmit', newPDrawSub);
            }
        },

        validateFormField(rules, value) {

            let validates = true;
            let errorMessage = null;

            for (const rule in rules) {

                validates = rules[rule].regex.test(value);

                if(validates === false){
                    
                    errorMessage = rules[rule].message;
                    break;
                }
            }

            return { validates: validates, errorMessage: errorMessage };
        },

        validateForm() {

            let validates = true;

            if(this.validationRules?.firstName) {

                const validationResult = this.validateFormField(this.validationRules.firstName, this.firstName);

                validates = validationResult.validates && validates;

                // need to show the error message
            }

            return validates;
        },


    }
}
</script>

<style scoped>

</style>