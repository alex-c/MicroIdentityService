<template>
  <div class="box">
    <header v-if="title !== undefined">
      {{ title }}
      <div class="actions">
        <slot name="actions" />
      </div>
    </header>
    <div class="box-body"><slot /></div>
  </div>
</template>

<script>
export default {
  name: 'Box',
  props: ['title'],
};
</script>

<style lang="scss" scoped>
@import '@/style/colors.scss';

.box {
  text-align: left;
  border: 1px solid $mis-color-border;
  border-radius: 3px;
  box-shadow: 1px 1px 2px 0px $mis-color-shadow;
  background-color: white;
  & > header {
    background-color: $mis-color-primary;
    padding: 16px;
    color: white;
    border-bottom: 1px solid $mis-color-border;
    & > .actions {
      float: right;
      & > .action:hover {
        cursor: pointer;
      }
    }
  }
}

.box-body {
  padding: 16px;
  & > .content-row {
    margin-top: 8px;
    overflow: auto;
    &:first-child {
      margin: 0;
    }
  }
}

.filter-row {
  display: flex;
  margin: 0px -4px;
  & > * {
    margin: 0px 4px;
  }
}
</style>
