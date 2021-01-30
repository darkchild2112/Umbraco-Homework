<template>
    <div>
        <form @submit="submitEntry">
            <Input v-model="firstName" ph="First t Name..." id="firstName" label="First Name" :validationRules="validationRules.firstName" />
            <Input v-model="lastName" id="lastName" ph="Last Name..." label="Last Name" :validationRules="validationRules.lastName"/>
            <Input v-model="email" id="email" ph="Email..." label="Email" :validationRules="validationRules.email"/>
            <Input v-model="serialNumber" id="serialNumber" ph="Serial Number..." label="Serial Number" :validationRules="validationRules.serialNumber"/>

            <input type="submit" value="submit" :disabled="!formIsValid" />
        </form>
    </div>
</template>

<script>
import Input from '@/components/UI/Input';

export default {
    name: "PrizeDrawForm",
    props: ['validationRules'],
    components: { Input },
    data() {

        return {
            firstName: { isValid: false, value: ''},
            lastName: { isValid: false, value: ''},
            email: { isValid: false, value: ''},
            serialNumber: { isValid: false, value: ''},
        }
    },
    computed: {
        formIsValid(){
            return this.firstName.isValid && this.lastName.isValid && this.email.isValid && this.serialNumber.isValid;
        }
    },
    methods: {

        updateConsole(value) {
            console.log(value)
        },
        submitEntry(e) {

            e.preventDefault();

            if(this.formIsValid === true)
            {
                const newPDrawSub = {

                    firstName: this.firstName.value,
                    lastName: this.lastName.value,
                    email: this.email.value,
                    serialNumber: this.serialNumber.value
                };

                this.$emit('successfulSubmit', newPDrawSub);
            }
        },

        validateForm() {

            this.formValid = this.firstName.isValid && this.lastName.isValid && this.email.isValid && this.serialNumber.isValid;
        },
    }
}
</script>

<style scoped>

</style>