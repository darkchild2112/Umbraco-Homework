<template>
    <div>
        <label :for="id" class="sr-only">{{ label }}</label>
        
        <input 
            type="text" 
            :value="modelValue.value" 
            @input="updateParent($event)" 
            :placeholder="ph"
            :id="id"
            :name="id"
            :class="validDisplay"
            class="form-control">
        <div v-if="isValid === false" class="error-text">{{ errorMsg }}</div>
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

                    this.isValid = re.test(value.toLowerCase());

                    if(this.isValid === false){
                        
                        this.errorMsg = this.validationRules[rule].errorMessage;
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

        border-color: #28a745;
    }

    .invalid {

        border-color: #dc3545;
    }

    .error-text {
        color: #dc3545;
        font-size: 0.75rem;
        margin-top: 5px;
    }

</style>