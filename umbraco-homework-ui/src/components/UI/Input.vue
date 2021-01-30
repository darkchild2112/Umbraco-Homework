<template>
    <div>
    <label :for="id">{{ label }}</label>
    <p v-if="isValid === false">{{ errorMsg }}</p>
    <input 
        type="text" 
        :value="modelValue.value" 
        @input="updateParent($event)" 
        :placeholder="ph"
        :id="id"
        :name="id"
        :class="validDisplay">
    </div>
</template>

<script>
export default {
    name: "Input",
    props: { 
        modelValue: Object, 
        ph: String, 
        id: String, 
        label:String, 
        validationRules: Object 
    },
    emits: ['update:modelValue'],
    data() {
        return { 
            isValid: null,
            errorMsg: ''
        }
    },
    computed: {
        validDisplay() {
            
            return this.isValid != null ? this.isValid === true ? 'valid' : 'invalid' : '';
        }
    },
    methods: {
        validateFormField(value) {

            if(this.validationRules) {

                for (const rule in this.validationRules) {

                    var re = new RegExp(this.validationRules[rule].regex);

                    this.isValid = re.test(value);

                    if(this.isValid === false){
                        
                        this.errorMsg = this.validationRules[rule].message;
                        break;
                    }
                }
            }
            else{
                this.isValid = true;
            }
        },
        updateParent(event){

            this.validateFormField(event.target.value)
            
            const result = {

                value: event.target.value,
                isValid: this.isValid 
            };

            this.$emit('update:modelValue', result);
        }

        
    }
}
</script>

<style scoped>

    .valid {

        border-color: green;
    }

    .invalid {

        border-color: red;
    }

</style>