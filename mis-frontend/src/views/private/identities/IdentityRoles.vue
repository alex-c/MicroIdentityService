<template>
  <div id="identity-roles" class="standard-page">
    {{ identity }}
  </div>
</template>

<script>
import Api from '@/Api.js';
import GenericErrorHandlingMixin from '@/mixins/GenericErrorHandlingMixin.js';

export default {
  name: 'IdentityRoles',
  data() {
    return {
      id: this.$route.params.id,
      identity: null,
    };
  },
  methods: {
    getIdentity: function() {
      Api.identities
        .getIdentity(this.id)
        .then(result => (this.identity = result.body))
        .catch(this.handleHttpError);
    },
  },
  mounted() {
    this.getIdentity();
  },
};
</script>
