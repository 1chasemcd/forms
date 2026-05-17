// using FormsApi.Contract;
// using FormsApi.Contract.View;

// namespace Tests.Builder.FormBuilder;

// public class ViewTests
// {
//     private readonly FormDto _form = new TestFormBuilder().Build();

//     [Test]
//     public void Build_SetsCorrectRootViewType()
//     {
//         Assert.That(_form.View, Is.InstanceOf<CombinedViewDto>());
//     }

//     [Test]
//     public void Build_SetsCorrectViewTitle()
//     {
//         Assert.That(_form.View.Title, Is.EqualTo(new ConstantDto("Title")));
//     }
// }
