<template>
  <div>
      <Container>
        <Header />
      </Container>

      <Container>

        <ErrorAlert v-if:="error != null" :error="error" />

        <PrizeDrawForm 
          v-if="this.formState === 'initial'" 
          v-bind:validationRules="this.validationRules"
          v-on:successfulSubmit="successfulPrizeDrawSubmit"/>

        <Spinner v-if="this.formState === 'sending'"/>

        <div v-if="this.formState === 'submitted'" class="text-center">
          <h2>Entry Successful</h2>
          <p>Thank you for submitting the form</p>
          <template v-if="submissions < config.maxSubmissions">
            <Button v-on:click="tryAgain" :text="'Submit Entry'" />
          </template>
        </div>
      </Container>
  </div>
</template>

<script>

import Header from '@/components/layout/Header'
import PrizeDrawForm from '@/components/PrizeDrawForm'

import Container from '@/components/layout/Container'
import Spinner from '@/components/UI/Spinner'

import ErrorAlert from '@/components/UI/ErrorAlert'

import Button from '@/components/UI/Button'

import dataAccess from '@/axios-base';

export default {
  name: 'Home',
  components: {
    Header,
    PrizeDrawForm,
    Container, 
    Spinner,
    ErrorAlert,
    Button
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
        },
        error: null
    }
  },
  methods: {

      updateInput(event) {

        console.log(event);
        this.formState = event;
      },
      successfulPrizeDrawSubmit(entry) {

        this.formState = 'sending';
        this.error = null;

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
              const error = { 
                summary: err.response.data.value.message,
                errors: err.response.data.value.errors
              }
              
              this.error = error;
            }

            this.formState = 'initial';
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
