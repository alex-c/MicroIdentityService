<template>
  <div class="menu-item" @click="navigate">
    <el-link :type="type">{{ label }}</el-link>
  </div>
</template>

<script>
export default {
  name: 'MenuItem',
  props: ['target', 'current'],
  data() {
    return {
      label: this.$t(`general.${this.target}`),
      route: `/${this.target}`,
    };
  },
  computed: {
    type() {
      return this.route === this.current ? 'primary' : 'default';
    },
  },
  methods: {
    navigate: function() {
      this.$router.push({ path: this.route });
    },
  },
};
</script>

<style lang="scss" scoped>
.menu-item {
  padding: 16px 16px 0px;
  & > .el-link {
    font-size: 16px;
  }
}
</style>
