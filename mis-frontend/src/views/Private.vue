<template>
  <div id="private">
    <header>
      <div id="header-title">MicroIdentityServer</div>
      <div id="header-options">
        <div class="option"><i class="el-icon-s-tools" /></div>
      </div>
    </header>
  </div>
</template>

<style lang="scss" scoped>
@import '@/style/colors.scss';

header {
  text-align: left;
  background-color: $mis-color-primary;
  overflow: auto;
  color: white;
  border-bottom: 1px solid $mis-color-border;
  box-shadow: 0px 1px 2px 0px $mis-color-shadow;
}

#header-title {
  float: left;
  font-size: 30px;
  padding: 16px;
  height: 32px;
}

#header-options {
  float: right;
  display: flex;
  & > .option {
    font-size: 32px;
    padding: 16px;
    width: 32px;
    height: 32px;
  }
  & > .option:hover {
    background-color: white;
    color: $mis-color-primary;
    cursor: pointer;
  }
}
</style>
