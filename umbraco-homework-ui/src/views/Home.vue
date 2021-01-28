<template>
  <div>
    <Header />
    <PrizeDrawForm 
      v-if="this.formState === 'initial'" 
      v-bind:validationRules="this.validationRules"
      v-on:successfulSubmit="successfulPrizeDrawSubmit"/>

    <div v-if="this.formState === 'submitted'">
      <p>Thank you for submitting the form</p>
      <button v-on:click="tryGain">Try again</button>
    </div>
  </div>
</template>

<script>

import Header from '@/components/layout/Header'
import PrizeDrawForm from '@/components/PrizeDrawForm'

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
        validationRules: {
          firstName: [{ regex: /\S/, message: 'first name is mandatory' }],
          lastName: [{ regex: /\S/, message: 'last name is mandatory' }],
          email: [{ regex: /\S/, message: 'email is mandatory' }],
          serialNumber: [{ regex: /\S/, message: 'serial number is mandatory' }]
        }
    }
  },
  methods: {

      successfulPrizeDrawSubmit(entry) {

        this.submissions = this.submissions += 1;
        this.formState = 'submitted';

        console.log('prize draw successfully submitted');
        console.log(`successfully submitted ${this.submissions} times`);
        console.log(entry);
      },

      tryGain()
      {
        this.formState = 'initial';
      }
  }
}

</script>
