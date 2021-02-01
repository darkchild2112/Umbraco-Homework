<template>
  <div>
    <Header />

    <PrizeDrawForm 
      v-if="this.formState === 'initial'" 
      v-bind:validationRules="this.validationRules"
      v-on:successfulSubmit="successfulPrizeDrawSubmit"/>

    <div v-if="this.formState === 'submitted'">
      <p>Thank you for submitting the form</p>
      <template v-if="submissions < config.maxSubmissions">
        <button v-on:click="tryAgain">Try again</button>
      </template>
    </div>
  </div>
</template>

<script>

import Header from '@/components/layout/Header'
import PrizeDrawForm from '@/components/PrizeDrawForm'

import dataAccess from '@/axios-base';

export default {
  name: 'Home',
  components: {
    Header,
    PrizeDrawForm
  },
  data() {
    return {
        submissions: 0,
        formState: 'initial',
        config: null,
        validationRules: {
          firstNameRules: null,
          lastNameRules: null,
          emailRules:  null,
          serialNumberRules: null
        }
    }
  },
  methods: {

      updateInput(event) {

        console.log(event);
        this.formState = event;
      },
      successfulPrizeDrawSubmit(entry) {

        dataAccess.post('/PrizeDraw/SubmitEntry', entry)
          .then(response => {

            console.log('Success:', response);

            this.submissions = this.submissions += 1;
            this.formState = 'submitted';

            console.log('prize draw successfully submitted');
            console.log(`successfully submitted ${this.submissions} times`);
            console.log(entry);
          })
          .catch(err => { 
            console.log(err.response);
            
            if(err.response.status === 400)
            {
              //const msg = err.response.data.value.message;
              //const errors = err.response.data.value.errors;
              
              console.log(err.response.data.value);
            }
          });
      },

      tryAgain()
      {
        this.formState = 'initial';
      }
  },
  mounted(){
      
      dataAccess.get('/Config')
        .then(response => {

          console.log(response);

          this.config = response.data;
          this.validationRules = response.data.validation;
        })
        .catch(err => console.log(err));
  }
}

</script>
