using Prog_part_2.Models;
using System.ComponentModel.DataAnnotations;

namespace Test2
{
    public class UnitTest1
    {

        ContractFile _validator = new ContractFile();  
        
        [Fact]
        public void IsValid_ValidPdfFile_ReturnsTrue()
        {
            var result = _validator.IsValid("document.pdf", 100000);

            Assert.True(result);
        }

        [Fact]
        public void IsValid_InvalidExtension_ReturnsFalse()
        {
            var result = _validator.IsValid("image.jpg", 100000);

            Assert.False(result);
        }

        [Fact]
        public void IsValid_EmptyFileName_ReturnsFalse()
        {
            var result = _validator.IsValid("", 100000);

            Assert.False(result);
        }

        [Fact]
        public void IsValid_FileTooLarge_ReturnsFalse()
        {
            var result = _validator.IsValid("file.pdf", 10_000_000);

            Assert.False(result);
        }
       
    }
}