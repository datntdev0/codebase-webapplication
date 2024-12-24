import { MyCodebaseTemplatePage } from './app.po';

describe('MyCodebase App', function() {
  let page: MyCodebaseTemplatePage;

  beforeEach(() => {
    page = new MyCodebaseTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
