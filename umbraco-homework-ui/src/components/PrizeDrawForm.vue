<template>
    <form @submit="submitEntry">

        <div class="form-row">
            <div class="form-group col-md-6">
                <Input type="text" v-model="firstName" ph="First Name..." id="firstName" label="First Name:" :validationRules="validationRules.firstNameRules" />
            </div>
            <div class="form-group col-md-6">
                <Input type="text" v-model="lastName" id="lastName" ph="Last Name..." label="Last Name:" :validationRules="validationRules.lastNameRules"/>
            </div>
        </div>

        <div class="form-row">
            <div class="form-group col-md-6">
                <Input type="text" v-model="dateOfBirth" ph="Date of Birth (mm/dd/yyyy)" id="dateOfBirth" label="Date of Birth:" :validationRules="validationRules.dateOfBirthRules" />
            </div>
            <div class="form-group col-md-6">
                <Input type="text" v-model="email" id="email" ph="Email..." label="Email:" :validationRules="validationRules.emailRules"/>
            </div>
        </div>

        <div class="form-row">
            <div class="form-group col-md-12">
                <Input type="text" v-model="serialNumber" id="serialNumber" ph="Serial Number..." label="Serial Number:" :validationRules="validationRules.serialNumberRules"/>
            </div>
        </div>
        
        

        <Button :text="'Submit Entry'" :enabled="!formIsValid" />
    </form>
</template>

<script>
import Input from '@/components/UI/Input';
import Button from '@/components/UI/Button'

import moment from 'moment';

export default {
    name: "PrizeDrawForm",
    props: {
        validationRules: Object
    },
    components: { Input, Button },
    data() {

        return {
            firstName: { isValid: false, value: ''},
            lastName: { isValid: false, value: ''},
            email: { isValid: false, value: ''},
            serialNumber: { isValid: false, value: ''},
            dateOfBirth : { isValid: false, value: ''}
        }
    },
    computed: {
        formIsValid(){
            return this.firstName.isValid && this.lastName.isValid && this.email.isValid && this.serialNumber.isValid;
        }
    },
    methods: {

        submitEntry(e) {

            e.preventDefault();

            if(this.formIsValid === true)
            {
                const dob = moment(`${this.dateOfBirth.value} 01:01:01`, 'DD/MM/YYYY hh:mm:ss');

                const newPDrawSub = {

                    firstName: this.firstName.value,
                    lastName: this.lastName.value,
                    email: this.email.value,
                    serialNumber: this.serialNumber.value,
                    dateOfBirth: dob.toDate()
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